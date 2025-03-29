using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Billing : IBilling
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Billing" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Billing(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<BillingInfo> GetInfoAsync(string accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"accounts/{accountId}/billing")
				.WithCancellationToken(cancellationToken)
				.AsObject<BillingInfo>();
		}

		/// <inheritdoc/>
		public async Task<BillingInvoiceSummary[]> GetInvoicesAsync(string accountId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
		{
			var response = await _client
				.GetAsync($"accounts/{accountId}/billing/invoices")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			var currency = response.GetPropertyValue("currency", string.Empty);
			var invoices = response.GetProperty("invoices", false)?.ToObject<BillingInvoiceSummary[]>() ?? Array.Empty<BillingInvoiceSummary>();
			Array.ForEach(invoices, invoice => invoice.Currency = currency);

			return invoices;
		}

		/// <inheritdoc/>
		public Task<BillingInvoiceDetails> GetInvoiceDetailsAsync(string accountId, string invoiceId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"accounts/{accountId}/billing/invoices/{invoiceId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<BillingInvoiceDetails>();
		}

		/// <inheritdoc/>
		public Task<BillingPlanUsageInfo> GetPlanUsageAsync(string accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"accounts/{accountId}/plans/usage")
				.WithCancellationToken(cancellationToken)
				.AsObject<BillingPlanUsageInfo>();
		}

		/// <inheritdoc/>
		public Task<Stream> DownloadInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default)
		{
			// Prepare the request
			var request = _client
			   .GetAsync($"billing/invoices/{invoiceId}/download")
			   .WithOptions(completeWhen: HttpCompletionOption.ResponseHeadersRead)
			   .WithCancellationToken(cancellationToken);

			// Remove our custom error handler because it reads the content of the response to check for error messages returned from the Zoom API.
			// This is problematic because we want the content of the response to be streamed.
			request = request.WithoutFilter<ZoomErrorHandler>();

			// We need to add the default error filter to throw an exception if the request fails.
			// The error handler is safe to use with streaming responses because it does not read the content to determine if an error occured.
			request = request.WithFilter(new DefaultErrorFilter());

			// Dispatch the request
			return request.AsStream();
		}
	}
}
