using Pathoschild.Http.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

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
		public Task<HttpResponseMessage> GetPlanUsageAsync(string accountId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"accounts/{accountId}/plans/usage")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
