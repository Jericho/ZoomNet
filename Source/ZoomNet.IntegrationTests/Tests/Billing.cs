using System;
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

			// GET BILLING INFO
			var billingInfo = await client.Billing.GetInfoAsync("me", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"The billing contact is: {billingInfo.FirstName} {billingInfo.LastName}").ConfigureAwait(false);

			// GET PLAN USAGE
			var planUsage = await client.Billing.GetPlanUsageAsync("me", cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"Your base plan type is: {planUsage.BasePlan.Type}").ConfigureAwait(false);

			// GET INVOICES
			var from = DateTime.UtcNow.AddYears(-1);
			var to = DateTime.UtcNow;
			var invoices = await client.Billing.GetInvoicesAsync("me", from, to, cancellationToken).ConfigureAwait(false);
			await log.WriteLineAsync($"{invoices.Length} invoices were issued in the last year.").ConfigureAwait(false);
		}
	}
}
