using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
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

		private WebsocketClient _websocketClient;
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

			var clientFactory = new Func<Uri, CancellationToken, Task<WebSocket>>(async (uri, cancellationToken) =>
			{
				_logger.LogTrace("Establishing connection to Zoom");

				// The current value in the uri parameter must be ignored because it contains "access_token" which may have expired.
				// The following line ensures the "access_token" is refreshed whenever it expires.
				uri = new Uri($"wss://ws.zoom.us/ws?subscriptionId={_subscriptionId}&access_token={_tokenHandler.Token}");

				var client = new ClientWebSocket()
				{
					Options =
					{
						KeepAliveInterval = TimeSpan.Zero, // Turn off built-in "Keep Alive" feature because Zoom uses proprietary "heartbeat" every 30 seconds rather than standard "ping/pong" messages at regular interval.
						Proxy = _proxy,
					}
				};
				client.Options.SetRequestHeader("ZoomNet-Version", ZoomClient.Version);

				await client.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);
				return client;
			});

			_websocketClient = new WebsocketClient(new Uri("wss://ws.zoom.us"), clientFactory)
			{
				Name = "ZoomNet",
				ReconnectTimeout = TimeSpan.FromSeconds(45), // Greater than 30 seconds because we send a heartbeat every 30 seconds
				ErrorReconnectTimeout = TimeSpan.FromSeconds(45)
			};
			_websocketClient.ReconnectionHappened.Subscribe(info => _logger.LogTrace("Reconnection happened, type: {reconnectionReason}", info.Type));
			_websocketClient.DisconnectionHappened.Subscribe(info => _logger.LogTrace("Disconnection happened, type: {disconnectionReason}", info.Type));
		}

		/// <summary>
		/// Start listening to incoming webhooks from Zoom.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Asynchronous task.</returns>
		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			_websocketClient.MessageReceived
				.Select(response => Observable.FromAsync(() => ProcessMessage(response, cancellationToken)))
				.Merge(5) // Allow up to 5 messages to be processed concurently. This number is arbitrary but it seems reasonable.
				.Subscribe();

			Task.Run(() => SendHeartbeat(_websocketClient, cancellationToken), cancellationToken);

			return _websocketClient.Start();
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

		private async Task SendHeartbeat(IWebsocketClient client, CancellationToken cancellationToken = default)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken); // Zoom requires a heartbeat every 30 seconds

				if (!client.IsRunning)
				{
					_logger.LogTrace("Client is not running. Skipping heartbeat");
					continue;
				}

				_logger.LogTrace("Sending heartbeat");

				await client.SendInstant("{\"module\":\"heartbeat\"}").ConfigureAwait(false);
			}
		}

		private async Task ProcessMessage(ResponseMessage msg, CancellationToken cancellationToken = default)
		{
			var jsonDoc = JsonDocument.Parse(msg.Text);
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
				if (_websocketClient.IsRunning)
				{
					_websocketClient.Stop(WebSocketCloseStatus.NormalClosure, "Shutting down").GetAwaiter().GetResult();
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
