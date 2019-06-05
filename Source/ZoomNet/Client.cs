using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using ZoomNet.Logging;
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

		private readonly bool _mustDisposeHttpClient;
		private readonly ZoomClientOptions _options;

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
		public static string Version { get; private set; }

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		public static string UserAgent { get; private set; }

		/// <summary>
		/// Gets the resource which allows you to manage sub accounts.
		/// </summary>
		/// <value>
		/// The accounts resource.
		/// </value>
		//public IAccounts Accounts { get; private set; }

		/// <summary>
		/// Gets the resource which allows you to manage billing information.
		/// </summary>
		/// <value>
		/// The billing resource.
		/// </value>
		//public IBillingInformation BillingInformation { get; private set; }

		/// <summary>
		/// Gets the resource which allows you to manage users.
		/// </summary>
		/// <value>
		/// The users resource.
		/// </value>
		//public IUsers Users { get; private set; }

		/// <summary>
		/// Gets the resource wich alloes you to manage roles.
		/// </summary>
		/// <value>
		/// The roles resource.
		/// </value>
		//public IRoles Roles { get; private set; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings.
		/// </summary>
		/// <value>
		/// The meetings resource.
		/// </value>
		//public IMeetings Meetings { get; private set; }

		/// <summary>
		/// Gets the resource which allows you to manage webinars.
		/// </summary>
		/// <value>
		/// The webinars resource.
		/// </value>
		//public IWebinars Webinars { get; private set; }

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes static members of the <see cref="Client"/> class.
		/// </summary>
		static Client()
		{
			Version = typeof(Client).GetTypeInfo().Assembly.GetName().Version.ToString(3);
#if DEBUG
			Version = "DEBUG";
#endif
			UserAgent = $"ZoomNet/{Version} (+https://github.com/Jericho/ZoomNet)";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="options">Options for the Zoom client.</param>
		public Client(string apiKey, string apiSecret, ZoomClientOptions options = null)
			: this(apiKey, apiSecret, null, false, options)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class with a specific proxy.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="options">Options for the Zoom client.</param>
		public Client(string apiKey, string apiSecret, IWebProxy proxy, ZoomClientOptions options = null)
			: this(apiKey, apiSecret, new HttpClient(new HttpClientHandler { Proxy = proxy, UseProxy = proxy != null }), true, options)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class with a specific handler.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="handler">TThe HTTP handler stack to use for sending requests.</param>
		/// <param name="options">Options for the Zoom client.</param>
		public Client(string apiKey, string apiSecret, HttpMessageHandler handler, ZoomClientOptions options = null)
			: this(apiKey, apiSecret, new HttpClient(handler), true, options)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class with a specific http client.
		/// </summary>
		/// <param name="apiKey">Your Zoom API Key.</param>
		/// <param name="apiSecret">Your Zoom API Secret.</param>
		/// <param name="httpClient">Allows you to inject your own HttpClient. This is useful, for example, to setup the HtppClient with a proxy.</param>
		/// <param name="options">Options for the Zoom client.</param>
		public Client(string apiKey, string apiSecret, HttpClient httpClient, ZoomClientOptions options = null)
			: this(apiKey, apiSecret, httpClient, false, options)
		{
		}

		private Client(string apiKey, string apiSecret, HttpClient httpClient, bool disposeClient, ZoomClientOptions options)
		{
			_mustDisposeHttpClient = disposeClient;
			_httpClient = httpClient;
			_options = options ?? GetDefaultOptions();

			Version = typeof(Client).GetTypeInfo().Assembly.GetName().Version.ToString(3);
#if DEBUG
			Version = "DEBUG";
#endif

			_fluentClient = new FluentClient(new Uri(ZOOM_V2_BASE_URI), httpClient)
				.SetUserAgent(Client.UserAgent);
			// .SetRequestCoordinator(new ZoomRetryStrategy());

			_fluentClient.Filters.Remove<DefaultErrorFilter>();

			// Order is important: JwtTokenHandler, must be first, followed by DiagnosticHandler and then by ErrorHandler.
			// Also, the list of filters must be kept in sync with the filters in Utils.GetFluentClient in the unit testing project.
			_fluentClient.Filters.Add(new JwtTokenHandler(apiKey, apiSecret));
			_fluentClient.Filters.Add(new DiagnosticHandler(_options.LogLevelSuccessfulCalls, _options.LogLevelFailedCalls));
			_fluentClient.Filters.Add(new ZoomErrorHandler());

			_fluentClient.SetOptions(new FluentClientOptions()
			{
				IgnoreHttpErrors = false,
				IgnoreNullArguments = true
			});

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
				LogLevelFailedCalls = LogLevel.Debug
			};
		}

		#endregion
	}
}
