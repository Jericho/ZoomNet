using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.IntegrationTests
{
	internal class TestsRunner : IHostedService
	{
		private readonly IHostApplicationLifetime _hostApplicationLifetime;
		private readonly TestSuite _testSuite;

		public TestsRunner(IHostApplicationLifetime hostApplicationLifetime, TestSuite testSuite)
		{
			_hostApplicationLifetime = hostApplicationLifetime;
			_testSuite = testSuite;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			// Run the tests
			await Console.Out.WriteLineAsync($"Running integration tests: {_testSuite.GetType().Name}...").ConfigureAwait(false);
			await _testSuite.RunTestsAsync(cancellationToken).ConfigureAwait(false);
			await Console.Out.WriteLineAsync("Integration tests completed").ConfigureAwait(false);

			// Shutdown the application
			_hostApplicationLifetime.StopApplication();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
