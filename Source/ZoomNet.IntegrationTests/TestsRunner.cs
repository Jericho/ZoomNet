using Microsoft.Extensions.Hosting;
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
			await _testSuite.RunTestsAsync(cancellationToken).ConfigureAwait(false);

			// Shutdown the application
			_hostApplicationLifetime.StopApplication();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
