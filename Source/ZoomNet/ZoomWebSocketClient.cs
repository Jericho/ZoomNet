using ApiSharp.Models;
using ApiSharp.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// Client for Zoom's WebSocket webhooks.
	/// </summary>
	public class ZoomWebSocketClient : IDisposable
	{
		// The error message we get from Zoom's WebSocket server when a token has expired is not always the same.
		private static readonly HashSet<string> _invalidTokenMessages = new(StringComparer.OrdinalIgnoreCase)
		{
			"Invalid Token",
			"Token is invalid"
		};

		private readonly string _subscriptionId;
		private readonly ILogger _logger;
		private readonly IWebProxy _proxy;
		private readonly Func<Models.Webhooks.Event, CancellationToken, Task> _eventProcessor;
		private readonly bool _throwWhenUnknownEventType;

		private WebSocketClient _websocketClient;
		private HttpClient _httpClient;
		private ITokenHandler _tokenHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomWebSocketClient"/> class.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="subscriptionId">Your subscirption Id.</param>
		/// <param name="eventProcessor">A delegate that will be invoked when a wehook message is received.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="logger">Logger.</param>
		public ZoomWebSocketClient(IConnectionInfo connectionInfo, string subscriptionId, Func<Models.Webhooks.Event, CancellationToken, Task> eventProcessor, IWebProxy proxy = null, ILogger logger = null)
			: this(connectionInfo, subscriptionId, eventProcessor, false, proxy, logger)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomWebSocketClient"/> class.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <param name="subscriptionId">Your subscirption Id.</param>
		/// <param name="eventProcessor">A delegate that will be invoked when a wehook message is received.</param>
		/// <param name="throwWhenUnknownEventType">Indicates whether an exception should be thrown when an unknown event type is encountered.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="logger">Logger.</param>
		public ZoomWebSocketClient(IConnectionInfo connectionInfo, string subscriptionId, Func<Models.Webhooks.Event, CancellationToken, Task> eventProcessor, bool throwWhenUnknownEventType, IWebProxy proxy = null, ILogger logger = null)
		{
			ArgumentNullException.ThrowIfNull(connectionInfo);
			ArgumentNullException.ThrowIfNullOrWhiteSpace(subscriptionId);
			ArgumentNullException.ThrowIfNull(eventProcessor);

			// According to https://marketplace.zoom.us/docs/api-reference/websockets/, only Server-to-Server OAuth connections are supported
			if (connectionInfo is not OAuthConnectionInfo oAuthConnectionInfo || oAuthConnectionInfo.GrantType != OAuthGrantType.AccountCredentials)
			{
				throw new ArgumentException("Zoom's websocket server only supports Server-to-Server OAuth connections");
			}

			_subscriptionId = subscriptionId;
			_eventProcessor = eventProcessor;
			_proxy = proxy;
			_logger = logger ?? NullLogger.Instance;
			_httpClient = new HttpClient(new HttpClientHandler { Proxy = _proxy, UseProxy = _proxy != null });
			_tokenHandler = new OAuthTokenHandler(oAuthConnectionInfo, _httpClient);
			_throwWhenUnknownEventType = throwWhenUnknownEventType;

			const string zoomWebsocketBaseUrl = "wss://ws.zoom.us/ws";
			var proxyUri = _proxy?.GetProxy(new Uri(zoomWebsocketBaseUrl));
			var proxyCredentialsParts = proxyUri?.UserInfo?.Split([':'], StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
			var proxyUserName = proxyCredentialsParts.Length > 0 ? WebUtility.UrlDecode(proxyCredentialsParts[0]) : null;
			var proxyPassword = proxyCredentialsParts.Length > 1 ? WebUtility.UrlDecode(proxyCredentialsParts[1]) : null;

			var urlFactory = new Func<Task<Uri>>(async () =>
			{
				// The following line ensures the "access_token" is refreshed whenever it expires.
				return new Uri($"{zoomWebsocketBaseUrl}?subscriptionId={_subscriptionId}&access_token={_tokenHandler.Token}");
			});

			var websocketParameters = new WebSocketParameters(urlFactory().GetAwaiter().GetResult(), true)
			{
				Headers = new Dictionary<string, string>
				{
					{ "ZoomNet-Version", ZoomClient.Version }
				},
				KeepAliveInterval = TimeSpan.Zero, // Turn off built-in "Keep Alive" feature because Zoom uses proprietary "heartbeat" every 30 seconds rather than standard "ping/pong" messages at regular interval.
				ReconnectInterval = TimeSpan.FromSeconds(45), // Greater than 30 seconds because we send a heartbeat every 30 seconds
				Timeout = TimeSpan.FromSeconds(45), // Greater than 30 seconds because we send a heartbeat every 30 seconds
			};

			if (proxyUri != null)
			{
				websocketParameters.Proxy = new ProxyCredentials(proxyUri.Scheme + "://" + proxyUri.DnsSafeHost, proxyUri.Port, proxyUserName, proxyPassword);
			}

			_websocketClient = new WebSocketClient(_logger, websocketParameters)
			{
				GetReconnectionUrl = urlFactory
			};
			_websocketClient.OnReconnected += () => _logger.LogTrace("Connection reconnected");
			_websocketClient.OnClose += () => _logger.LogWarning("Connection closed");
			_websocketClient.OnError += ex => _logger.LogError(ex, "An error occurred in the WebSocket client");
			_websocketClient.OnOpen += () => _logger.LogTrace("Connection opened");
			_websocketClient.OnMessage += async msg => await ProcessMessage(msg).ConfigureAwait(false);
		}

		/// <summary>
		/// Start listening to incoming webhooks from Zoom.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Asynchronous task.</returns>
		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			Task.Run(() => SendHeartbeat(_websocketClient, cancellationToken), cancellationToken);

			return _websocketClient.ConnectAsync();
		}

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

		private async Task SendHeartbeat(WebSocketClient webSocketClient, CancellationToken cancellationToken = default)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken); // Zoom requires a heartbeat every 30 seconds

				if (!webSocketClient.IsOpen)
				{
					_logger.LogTrace("Web socket connection is not opened. Skipping heartbeat");
					continue;
				}

				_logger.LogTrace("Sending heartbeat");

				webSocketClient.Send("{\"module\":\"heartbeat\"}");
			}
		}

		private async Task ProcessMessage(string msg, CancellationToken cancellationToken = default)
		{
			var jsonDoc = JsonDocument.Parse(msg);
			var module = jsonDoc.RootElement.GetPropertyValue("module", string.Empty);
			var success = jsonDoc.RootElement.GetPropertyValue("success", true);
			var content = jsonDoc.RootElement.GetPropertyValue("content", string.Empty);

			if (_invalidTokenMessages.Contains(content))
			{
				_logger.LogTrace("{module}. Token is invalid (presumably expired). Requesting a new token...", module);
				_tokenHandler.RefreshTokenIfNecessary(true);
				return;
			}
			else if (!success)
			{
				_logger.LogTrace("FAILURE: Received message: {module}. {content}", module, content);
				return;
			}

			switch (module)
			{
				case "build_connection":
					_logger.LogTrace("Received message: {module}. Connection has been established.", module);
					break;
				case "heartbeat":
					_logger.LogTrace("Received message: {module}. Server is acknowledging heartbeat.", module);
					break;
				case "message":
					var parser = new WebhookParser(_throwWhenUnknownEventType);
					var webhookEvent = parser.ParseEventWebhook(content);
					var eventType = webhookEvent.EventType;
					_logger.LogTrace("Received webhook event: {eventType}", eventType);
					try
					{
						await _eventProcessor(webhookEvent, cancellationToken).ConfigureAwait(false);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while processing webhook event: {eventType}", eventType);
					}

					break;
				default:
					_logger.LogError("Received unknown message: {module}", module);
					break;
			}
		}

		private void ReleaseManagedResources()
		{
			_tokenHandler = null;

			if (_websocketClient != null)
			{
				if (_websocketClient.IsOpen)
				{
					_websocketClient.CloseAsync().GetAwaiter().GetResult();
				}

				_websocketClient.Dispose();
				_websocketClient = null;
			}

			_httpClient?.Dispose();
			_httpClient = null;
		}

		private void ReleaseUnmanagedResources()
		{
			// We do not hold references to unmanaged resources
		}
	}
}
