using System.IO;
using System.Net.Http;
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
		/// Retrieve information on plan usage for an account.
		/// </summary>
		/// <param name="accountId">Unique identifier of the account.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// PLACEHOLDER: this method must return a strongly-typed response.
		/// </returns>
		Task<HttpResponseMessage> GetPlanUsageAsync(string accountId, CancellationToken cancellationToken = default);
	}
}
