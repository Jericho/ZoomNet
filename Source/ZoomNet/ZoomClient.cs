using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using ZoomNet.Json;
using ZoomNet.Resources;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// REST client for interacting with Zoom's API.
	/// </summary>
	/// <remarks>
	/// Don't be fooled by the fact that this class implements the IDisposable interface: it is not meant to be short-lived and instantiated with every request.
	/// It is meant to be long-lived and re-used throughout the life of an application.
	/// The reason is: we use Microsoft's HttpClient to dispatch requests which itself is meant to be long-lived and re-used.
	/// Instantiating an HttpClient class for every request will exhaust the number of sockets available under heavy loads and will result in SocketException errors.
	///
	/// See <a href="https://github.com/Jericho/ZoomNet/issues/35">this discussion</a> for more information about managing the lifetime of your client instance.
	/// </remarks>
	public class ZoomClient : IZoomClient, IDisposable
	{
		#region FIELDS

		private static string _version;

		private readonly bool _mustDisposeHttpClient;
		private readonly ZoomClientOptions _options;
		private readonly ILogger _logger;

		private HttpClient _httpClient;
		private IClient _fluentClient;

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
					_version = typeof(ZoomClient).GetTypeInfo().Assembly.GetName().Version.ToString(3);
#if DEBUG
					_version = "DEBUG";
#endif
				}

				return _version;
			}
		}

		/// <inheritdoc/>
		public IAccounts Accounts { get; private set; }

		/// <inheritdoc/>
		public IBilling Billing { get; private set; }

		/// <inheritdoc/>
		public ICallLogs CallLogs { get; private set; }

		/// <inheritdoc/>
		public IChat Chat { get; private set; }

		/// <inheritdoc/>
		public IChatbot Chatbot { get; private set; }

		/// <inheritdoc/>
		public ICloudRecordings CloudRecordings { get; private set; }

		/// <inheritdoc/>
		public IContacts Contacts { get; private set; }

		/// <inheritdoc/>
		public IDashboards Dashboards { get; private set; }

		/// <inheritdoc/>
		[Obsolete("The Data Compliance API is deprecated")]
		public IDataCompliance DataCompliance { get; private set; }

		/// <inheritdoc/>
		public IEvents Events { get; private set; }

		/// <inheritdoc/>
		public IExternalContacts ExternalContacts { get; private set; }

		/// <inheritdoc/>
		public IGroups Groups { get; private set; }

		/// <inheritdoc/>
		public IMeetings Meetings { get; private set; }

		/// <inheritdoc/>
		public IPastMeetings PastMeetings { get; private set; }

		/// <inheritdoc/>
		public IPastWebinars PastWebinars { get; private set; }

		/// <inheritdoc/>
		public IPhone Phone { get; private set; }

		/// <inheritdoc/>
		public IReports Reports { get; private set; }

		/// <inheritdoc/>
		public IRoles Roles { get; private set; }

		/// <inheritdoc/>
		public IRooms Rooms { get; private set; }

		/// <inheritdoc/>
		public ISms Sms { get; private set; }

		/// <inheritdoc/>
		public ITrackingFields TrackingFields { get; private set; }

		/// <inheritdoc/>
		public IUsers Users { get; private set; }

		/// <inheritdoc/>
		public IWebinars Webinars { get; private set; }

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomClient"/> class.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomClient(IConnectionInfo connectionInfo, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, new HttpClient(), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomClient"/> class with a specific proxy.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomClient(IConnectionInfo connectionInfo, IWebProxy proxy, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, new HttpClient(new HttpClientHandler { Proxy = proxy, UseProxy = proxy != null }), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomClient"/> class with a specific handler.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="handler">TThe HTTP handler stack to use for sending requests.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomClient(IConnectionInfo connectionInfo, HttpMessageHandler handler, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, new HttpClient(handler), true, options, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomClient"/> class with a specific http client.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="httpClient">Allows you to inject your own HttpClient. This is useful, for example, to setup the HtppClient with a proxy.</param>
		/// <param name="options">Options for the Zoom client.</param>
		/// <param name="logger">Logger.</param>
		public ZoomClient(IConnectionInfo connectionInfo, HttpClient httpClient, ZoomClientOptions options = null, ILogger logger = null)
			: this(connectionInfo, httpClient, false, options, logger)
		{
		}

		private ZoomClient(IConnectionInfo connectionInfo, HttpClient httpClient, bool disposeClient, ZoomClientOptions options, ILogger logger = null)
		{
			ArgumentNullException.ThrowIfNull(connectionInfo);
			ArgumentNullException.ThrowIfNull(httpClient);

			_mustDisposeHttpClient = disposeClient;
			_httpClient = httpClient;
			_options = options ?? new();
			_logger = logger ?? NullLogger.Instance;
			_fluentClient = new FluentClient(_options.ApiBaseUrl ?? throw new Exception("You must specify the API base URL"), httpClient)
				.SetUserAgent($"ZoomNet/{Version} (+https://github.com/Jericho/ZoomNet)");

			_fluentClient.Filters.Remove<DefaultErrorFilter>();

			// Remove all the built-in formatters and replace them with our custom JSON formatter
			_fluentClient.Formatters.Clear();
			_fluentClient.Formatters.Add(new JsonFormatter());

			// Order is important: the token handler must be first, followed by DiagnosticHandler and then by ErrorHandler.
			if (connectionInfo is OAuthConnectionInfo oAuthConnectionInfo)
			{
				var tokenHandler = new OAuthTokenHandler(oAuthConnectionInfo, httpClient);
				_fluentClient.Filters.Add(tokenHandler);
				_fluentClient.SetRequestCoordinator(new ZoomRetryCoordinator(new Http429RetryStrategy(), tokenHandler));
			}
			else
			{
				throw new ZoomException($"{connectionInfo.GetType()} is an unknown connection type", null, null, null, null);
			}

			// The list of filters must be kept in sync with the filters in Utils.GetFluentClient in the unit testing project.
			_fluentClient.Filters.Add(new DiagnosticHandler(_options.LogLevelSuccessfulCalls, _options.LogLevelFailedCalls, _logger));
			_fluentClient.Filters.Add(new ZoomErrorHandler());

			Accounts = new Accounts(_fluentClient);
			Billing = new Billing(_fluentClient);
			CallLogs = new CallLogs(_fluentClient);
			Chat = new Chat(_fluentClient);
			Chatbot = new Chatbot(_fluentClient);
			CloudRecordings = new CloudRecordings(_fluentClient);
			Contacts = new Contacts(_fluentClient);
			Dashboards = new Dashboards(_fluentClient);
			DataCompliance = new DataCompliance(_fluentClient);
			Events = new Events(_fluentClient);
			ExternalContacts = new ExternalContacts(_fluentClient);
			Groups = new Groups(_fluentClient);
			Meetings = new Meetings(_fluentClient);
			PastMeetings = new PastMeetings(_fluentClient);
			PastWebinars = new PastWebinars(_fluentClient);
			Phone = new Phone(_fluentClient);
			Reports = new Reports(_fluentClient);
			Roles = new Roles(_fluentClient);
			Rooms = new Rooms(_fluentClient);
			Sms = new Sms(_fluentClient);
			TrackingFields = new TrackingFields(_fluentClient);
			Users = new Users(_fluentClient);
			Webinars = new Webinars(_fluentClient);
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="ZoomClient"/> class.
		/// </summary>
		~ZoomClient()
		{
			// The object went out of scope and finalized is called.
			// Call 'Dispose' to release unmanaged resources
			// Managed resources will be released when GC runs the next time.
			Dispose(false);
		}

		#endregion

		#region PUBLIC METHODS

		/// <inheritdoc/>
		public bool HasPermissions(IEnumerable<string> scopes)
		{
			var tokenHandler = _fluentClient.Filters.OfType<OAuthTokenHandler>().SingleOrDefault();
			if (tokenHandler == null) throw new Exception("The concept of scopes only applies when using an OAuth connection.");

			// Ensure the token (and by extension the scopes) is not expired
			tokenHandler.RefreshTokenIfNecessary(false);

			// The list of scopes can be empty if a previously issued token was specified when the OAuthConnectionInfo was instantiated.
			// I am not aware of any way to fetch the list of scopes which would enable me to populate the list of scopes in the OAuthConnectionInfo.
			// Therefore in this scenario the only workaround I can think of is to force the token to be refreshed.
			var oAuthConnectionInfo = (OAuthConnectionInfo)tokenHandler.ConnectionInfo;
			if (oAuthConnectionInfo.Scopes == null) tokenHandler.RefreshTokenIfNecessary(true); // Force the token to be refreshed wich will have the side-effect of populating the '.Scopes'

			var missingScopes = scopes.Except(((OAuthConnectionInfo)tokenHandler.ConnectionInfo).Scopes).ToArray();
			return !missingScopes.Any();
		}

		/// <inheritdoc/>
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

		#endregion
	}
}
