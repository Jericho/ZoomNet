using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models.Webhooks;

namespace ZoomNet.IntegrationTests.TestSuites
{
	internal class WebSocketsTestSuite : TestSuite
	{
		private readonly IConnectionInfo _connectionInfo;
		private readonly string _subscriptionId;
		private readonly IWebProxy _proxy;

		public WebSocketsTestSuite(IConnectionInfo connectionInfo, string subscriptionId, IWebProxy proxy, ILoggerFactory loggerFactory) :
			base(null, loggerFactory, Array.Empty<Type>(), false)
		{
			_connectionInfo = connectionInfo;
			_subscriptionId = subscriptionId;
			_proxy = proxy;
		}

		public override async Task<ResultCodes> RunTestsAsync(CancellationToken cancellationToken)
		{
			var logger = base.LoggerFactory.CreateLogger<ZoomWebSocketClient>();
			var eventProcessor = new Func<Event, CancellationToken, Task>(async (webhookEvent, cancellationToken) =>
			{
				logger.LogInformation("Processing {eventType} event...", webhookEvent.EventType);
				await Task.Delay(1, cancellationToken).ConfigureAwait(false); // This async call gets rid of "CS1998 This async method lacks 'await' operators and will run synchronously".
			});

			// Configure cancellation (this allows you to press CTRL+C or CTRL+Break to stop the websocket client)
			var exitEvent = new ManualResetEvent(false);
			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				exitEvent.Set();
			};

			// Start the websocket client
			using var client = new ZoomWebSocketClient(_connectionInfo, _subscriptionId, eventProcessor, _proxy, logger);
			await client.StartAsync(cancellationToken).ConfigureAwait(false);
			exitEvent.WaitOne();

			return ResultCodes.Success;
		}
	}
}
