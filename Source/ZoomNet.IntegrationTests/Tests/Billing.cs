using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.IntegrationTests.Tests
{
	public class Billing : IIntegrationTest
	{
		public async Task RunAsync(User myUser, string[] myPermissions, IZoomClient client, TextWriter log, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			await log.WriteLineAsync("\n***** BILLING *****\n").ConfigureAwait(false);

			// GET PLAN USAGE
			var planUsage = await client.Billing.GetPlanUsageAsync("me", cancellationToken).ConfigureAwait(false);
		}
	}
}
