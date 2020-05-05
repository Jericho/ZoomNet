using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// REST client for interacting with ZoomNet's API.
	/// </summary>
	public class Client : IClient, IDisposable
	{
		#region FIELDS

		private const string ZOOM_V2_BASE_URI = "https://api.zoom.us/v2";

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
					_version = typeof(Client).GetTypeInfo().Assembly.GetName().Version.ToString(3);
#if DEBUG
					_version = "DEBUG";
#endif
				}

				return _version;
			}
		}

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		public static string UserAgent { get; private set; }

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public Client(string apiKey, string apiSecret, ZoomClientOptions options = null, ILogger logger = null)
			: this(apiKey, apiSecret, null, false, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class with a specific proxy.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public Client(string apiKey, string apiSecret, IWebProxy proxy, ZoomClientOptions options = null, ILogger logger = null)
			: this(apiKey, apiSecret, new HttpClient(new HttpClientHandler { Proxy = proxy, UseProxy = proxy != null }), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class with a specific handler.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="handler">TThe HTTP handler stack to use for sending requests.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public Client(string apiKey, string apiSecret, HttpMessageHandler handler, ZoomClientOptions options = null, ILogger logger = null)
			: this(apiKey, apiSecret, new HttpClient(handler), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class with a specific http client.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="httpClient">Allows you to inject your own HttpClient. This is useful, for example, to setup the HtppClient with a proxy.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public Client(string apiKey, string apiSecret, HttpClient httpClient, ZoomClientOptions options = null, ILogger logger = null)
			: this(apiKey, apiSecret, httpClient, false, options, logger)
		{
		}

		private Client(string apiKey, string apiSecret, HttpClient httpClient, bool disposeClient, ZoomClientOptions options, ILogger logger = null)
		{
			_mustDisposeHttpClient = disposeClient;
			_httpClient = httpClient;
			_options = options ?? GetDefaultOptions();
			_logger = logger ?? NullLogger.Instance;
			_fluentClient = new FluentClient(new Uri(ZOOM_V2_BASE_URI), httpClient)
				.SetUserAgent($"ZoomNet/{Version} (+https://github.com/Jericho/ZoomNet)")
				.SetRequestCoordinator(new ZoomRetryStrategy());

			_fluentClient.Filters.Remove<DefaultErrorFilter>();

			// Order is important: JwtTokenHandler, must be first, followed by DiagnosticHandler and then by ErrorHandler.
			// Also, the list of filters must be kept in sync with the filters in Utils.GetFluentClient in the unit testing project.
			_fluentClient.Filters.Add(new JwtTokenHandler(apiKey, apiSecret));
			_fluentClient.Filters.Add(new DiagnosticHandler(_options.LogLevelSuccessfulCalls, _options.LogLevelFailedCalls));
			_fluentClient.Filters.Add(new ZoomErrorHandler());

			//Accounts = new Accounts(_fluentClient);
			//BillingInformation = new BillingInformation(_fluentClient);
			//Users = new Users(_fluentClient);
			//Roles = new Roles(_fluentClient);
			//Meetings = new Meetings(_fluentClient);
			//Webinars = new Webinars(_fluentClient);
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="Client"/> class.
		/// </summary>
		~Client()
		{
			// The object went out of scope and finalized is called.
			// Call 'Dispose' to release unmanaged resources
			// Managed resources will be released when GC runs the next time.
			Dispose(false);
		}

		#endregion

		#region PUBLIC METHODS

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

		private ZoomClientOptions GetDefaultOptions()
		{
			return new ZoomClientOptions()
			{
				LogLevelSuccessfulCalls = LogLevel.Debug,
				LogLevelFailedCalls = LogLevel.Error
			};
		}

		#endregion
	}
}
