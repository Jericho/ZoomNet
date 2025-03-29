using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows developers with master accounts (also known as "primary accounts") to get information about billing plans of their accounts and subaccounts.
	/// </summary>
	/// <remarks>
	/// See <a href="https://developers.zoom.us/docs/api/rest/reference/billing/ma/#overview">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IBilling
	{
		/// <summary>
		/// Get a subaccount's billing information.
		/// </summary>
		/// <param name="accountId">Unique identifier of the account.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The billing information.
		/// </returns>
		Task<BillingInfo> GetInfoAsync(string accountId, CancellationToken cancellationToken = default);

		/// <summary>
		/// List a Zoom account's invoices.
		/// </summary>
		/// <param name="accountId">Unique identifier of the account.</param>
		/// <param name="from">Start date for the invoice query. The date range defined by the "from" and "to" parameters should not exceed one year. The range defined should fall within the past three years.</param>
		/// <param name="to">End date for the invoice query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The currency used in the invoices and an array containing summary information about invoices.
		/// </returns>
		Task<BillingInvoiceSummary[]> GetInvoicesAsync(string accountId, DateTime from, DateTime to, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get detailed information about an invoice.
		/// </summary>
		/// <param name="accountId">Unique identifier of the account.</param>
		/// <param name="invoiceId">Unique identifier of the invoice.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The details about the invoice.
		/// </returns>
		Task<BillingInvoiceDetails> GetInvoiceDetailsAsync(string accountId, string invoiceId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve information on plan usage for an account.
		/// </summary>
		/// <param name="accountId">Unique identifier of the account.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// PLACEHOLDER: this method must return a strongly-typed response.
		/// </returns>
		Task<BillingPlanUsageInfo> GetPlanUsageAsync(string accountId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Download a Zoom account's billed invoice file, in PDF format.
		/// </summary>
		/// <param name="invoiceId">The account's invoice ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// The <see cref="Stream"/> containing the file.
		/// </returns>
		/// <remarks>
		/// You may send one request to this API every 30 minutes until you reach the daily limit of 100 requests per account.
		/// </remarks>
		Task<Stream> DownloadInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default);
	}
}
