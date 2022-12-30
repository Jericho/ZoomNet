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
		public Task<PaginatedResponseWithToken<AppInfo>> GetPublicAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return GetAppsAsync<AppInfo>("public", recordsPerPage, pagingToken, cancellationToken);
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<AppInfo>> GetCreatedAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return GetAppsAsync<AppInfo>("account_created", recordsPerPage, pagingToken, cancellationToken);
		}

		/// <inheritdoc/>
		public async Task<AppInfoDetailed> GetAppInfoAsync(string appId, CancellationToken cancellationToken = default)
		{
			var appInfo = await _client
				.GetAsync($"marketplace/apps/{appId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<AppInfoDetailed>()
				.ConfigureAwait(false);

			// Set the Id because, surprisingly, the Zoom API does not include it in the response.
			appInfo.Id = appId;

			return appInfo;
		}

		private Task<PaginatedResponseWithToken<T>> GetAppsAsync<T>(string type, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"marketplace/apps")
				.WithArgument("type", type)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<T>("apps");
		}
	}
}
