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
		private readonly string _subscriptionId;

		public WebSocketsTestSuite(IConnectionInfo connectionInfo, string subscriptionId, IWebProxy proxy, ILoggerFactory loggerFactory) :
			base(connectionInfo, proxy, loggerFactory, Array.Empty<Type>(), false)
		{
			_subscriptionId = subscriptionId;
		}

		public override async Task<ResultCodes> RunTestsAsync()
		{
			var logger = base.LoggerFactory.CreateLogger<ZoomWebSocketClient>();
			var eventProcessor = new Func<Event, CancellationToken, Task>(async (webhookEvent, cancellationToken) =>
			{
				logger.LogInformation("Processing {eventType} event...", webhookEvent.EventType);
				await Task.Delay(1, cancellationToken).ConfigureAwait(false); // This async call gets rid of "CS1998 This async method lacks 'await' operators and will run synchronously".
			});

			// Configure cancellation (this allows you to press CTRL+C or CTRL+Break to stop the websocket client)
			var cts = new CancellationTokenSource();
			var exitEvent = new ManualResetEvent(false);
			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				cts.Cancel();
				exitEvent.Set();
			};

			// Start the websocket client
			using var client = new ZoomWebSocketClient(base.ConnectionInfo, _subscriptionId, eventProcessor, base.Proxy, logger);
			await client.StartAsync(cts.Token).ConfigureAwait(false);
			exitEvent.WaitOne();

			return ResultCodes.Success;
		}
	}
}
