using Pathoschild.Http.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Marketplace : IMarketplace
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Marketplace" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Marketplace(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<AppInfo>> GetUserAppRequestsAsync(string userId, AppRequestType requestType, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"marketplace/users/{userId}/apps")
				.WithArgument("type", requestType.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<AppInfo>("apps");
		}

		/// <inheritdoc/>
		public async Task<long[]> GetUserEntitlementsAsync(string userId, CancellationToken cancellationToken = default)
		{
			var result = await _client
				.GetAsync($"marketplace/users/{userId}/entitlements")
				.WithCancellationToken(cancellationToken)
				.AsRawJsonDocument("entitlements")
				.ConfigureAwait(false);

			var entitlements = result.RootElement
				.EnumerateArray()
				.Select(element => element.GetPropertyValue<long>("entitlement_id"))
				.ToArray();

			return entitlements;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<AppRequest>> GetAppRequestsAsync(string appId, AppRequestStatus status, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"marketplace/apps/{appId}/requests")
				.WithArgument("status", status.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<AppRequest>("requests");
		}



		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<AppInfo>> GetPublicAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return GetAppsAsync("public", recordsPerPage, pagingToken, cancellationToken);
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<AppInfo>> GetAccountAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return GetAppsAsync("account_created", recordsPerPage, pagingToken, cancellationToken);
		}

		private Task<PaginatedResponseWithToken<AppInfo>> GetAppsAsync(string type, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync("marketplace/apps")
				.WithArgument("type", type)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<AppInfo>("apps");
		}
	}
}
