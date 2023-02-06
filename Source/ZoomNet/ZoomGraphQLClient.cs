using GraphQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Json;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// Client for Zoom's GraphQL endpoint.
	/// </summary>
	public class ZoomGraphQLClient : IZoomGraphQLClient, IDisposable
	{
		#region FIELDS

		private const string ZOOM_GRAPHQL_BASE_URL = "https://api.zoom.us/v3/graphql";

		private static string _version;

		private readonly bool _mustDisposeHttpClient;
		private readonly ZoomClientOptions _options;
		private readonly ILogger _logger;

		private HttpClient _httpClient;
		private Pathoschild.Http.Client.IClient _fluentClient;

		#endregion

		#region PROPERTIES

		/// <summary>
		/// Gets the Version.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
		public static string Version
		{
			get
			{
				if (string.IsNullOrEmpty(_version))
				{
					_version = typeof(ZoomGraphQLClient).GetTypeInfo().Assembly.GetName().Version.ToString(3);
#if DEBUG
					_version = "DEBUG";
#endif
				}

				return _version;
			}
		}

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomGraphQLClient"/> class.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="options">Options for the Zoom GraphQL client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomGraphQLClient(IConnectionInfo connectionInfo, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, new HttpClient(), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomGraphQLClient"/> class with a specific proxy.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="options">Options for the Zoom GraphQL client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomGraphQLClient(IConnectionInfo connectionInfo, IWebProxy proxy, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, new HttpClient(new HttpClientHandler { Proxy = proxy, UseProxy = proxy != null }), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomGraphQLClient"/> class with a specific handler.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="handler">TThe HTTP handler stack to use for sending requests.</param>
		/// <param name="options">Options for the Zoom GraphQL client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomGraphQLClient(IConnectionInfo connectionInfo, HttpMessageHandler handler, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, new HttpClient(handler), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomGraphQLClient"/> class with a specific http client.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="httpClient">Allows you to inject your own HttpClient. This is useful, for example, to setup the HtppClient with a proxy.</param>
		/// <param name="options">Options for the Zoom GraphQL client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomGraphQLClient(IConnectionInfo connectionInfo, HttpClient httpClient, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, httpClient, false, options, logger)
		{
		}

		private ZoomGraphQLClient(IConnectionInfo connectionInfo, HttpClient httpClient, bool disposeClient, ZoomClientOptions options, ILogger logger = null)
		{
			// According to https://marketplace.zoom.us/docs/graphql/using-graphql/, any OAuth connection is supported
			if (connectionInfo == null) throw new ArgumentNullException(nameof(connectionInfo));
			if (connectionInfo is not OAuthConnectionInfo oAuthConnectionInfo)
			{
				throw new ArgumentException("GraphQL client only supports OAuth connections");
			}

			_mustDisposeHttpClient = disposeClient;
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_options = options ?? GetDefaultOptions();
			_logger = logger ?? NullLogger.Instance;
			_fluentClient = new FluentClient(new Uri(ZOOM_GRAPHQL_BASE_URL), httpClient)
				.SetUserAgent($"ZoomNet/{Version} (+https://github.com/Jericho/ZoomNet)");

			_fluentClient.Filters.Remove<DefaultErrorFilter>();

			// Remove all the built-in formatters and replace them with our custom JSON formatter
			_fluentClient.Formatters.Clear();
			_fluentClient.Formatters.Add(new JsonFormatter());

			// Order is important: the token handler (either JWT or OAuth) must be first, followed by DiagnosticHandler and then by ErrorHandler.
			var tokenHandler = new OAuthTokenHandler(oAuthConnectionInfo, httpClient);
			_fluentClient.Filters.Add(tokenHandler);
			_fluentClient.SetRequestCoordinator(new ZoomRetryCoordinator(new Http429RetryStrategy(), tokenHandler));

			// The list of filters must be kept in sync with the filters in Utils.GetFluentClient in the unit testing project.
			_fluentClient.Filters.Add(new DiagnosticHandler(_options.LogLevelSuccessfulCalls, _options.LogLevelFailedCalls, _logger));
			_fluentClient.Filters.Add(new ZoomErrorHandler());
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="ZoomGraphQLClient"/> class.
		/// </summary>
		~ZoomGraphQLClient()
		{
			// The object went out of scope and finalized is called.
			// Call 'Dispose' to release unmanaged resources
			// Managed resources will be released when GC runs the next time.
			Dispose(false);
		}

		#endregion

		#region PUBLIC METHODS

		/// <inheritdoc/>
		public Task<T> SendQueryAsync<T>(GraphQLRequest request, CancellationToken cancellationToken = default)
		{
			return _fluentClient
				.PostAsync(string.Empty)
				.WithCancellationToken(cancellationToken)
				.AsObject<T>("data");
		}

		/// <inheritdoc/>
		//public Task<GraphQLResponse<TResponse>> SendMutationAsync<TResponse>(GraphQLRequest request, CancellationToken cancellationToken = default)

		/// <inheritdoc/>
		//public IObservable<GraphQLResponse<TResponse>> CreateSubscriptionStream<TResponse>(GraphQLRequest request)

		/// <inheritdoc/>
		//public IObservable<GraphQLResponse<TResponse>> CreateSubscriptionStream<TResponse>(GraphQLRequest request, Action<Exception> exceptionHandler)

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// Call 'Dispose' to release resources
			Dispose(true);

			// Tell the GC that we have done the cleanup and there is nothing left for the Finalizer to do
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ReleaseManagedResources();
			}
			else
			{
				// The object went out of scope and the Finalizer has been called.
				// The GC will take care of releasing managed resources, therefore there is nothing to do here.
			}

			ReleaseUnmanagedResources();
		}

		#endregion

		#region PRIVATE METHODS

		private static ZoomClientOptions GetDefaultOptions()
		{
			return new ZoomClientOptions()
			{
				LogLevelSuccessfulCalls = LogLevel.Debug,
				LogLevelFailedCalls = LogLevel.Error
			};
		}

		private void ReleaseManagedResources()
		{
			if (_fluentClient != null)
			{
				_fluentClient.Dispose();
				_fluentClient = null;
			}

			if (_httpClient != null && _mustDisposeHttpClient)
			{
				_httpClient.Dispose();
				_httpClient = null;
			}
		}

		private void ReleaseUnmanagedResources()
		{
			// We do not hold references to unmanaged resources
		}

		#endregion
	}
}
