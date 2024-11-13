using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Marketplace : IMarketplace
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Marketplace" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Marketplace(IClient client)
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
				.AsJson("entitlements")
				.ConfigureAwait(false);

			var entitlements = result
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
		public Task<string> CreateEventSubscriptionAsync(IEnumerable<string> events, string subscriptionName, Uri webhookUrl, IEnumerable<string> userIds, string subscriptionScope, string accountId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "events", events.ToArray() },
				{ "event_subscription_name", subscriptionName },
				{ "event_webhook_url", webhookUrl.AbsoluteUri },
				{ "user_ids", userIds?.ToArray() },
				{ "subscription_scope",subscriptionScope },
				{ "account_id", accountId },
			};

			return _client
				.PostAsync("marketplace/app/event_subscription")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<string>("event_subscription_id");
		}

		/// <inheritdoc/>
		public Task<AppInfo> GetAppInfoAsync(string appId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"marketplace/apps/{appId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<AppInfo>();
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
