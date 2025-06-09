using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to to access information from Zoom. Also, allows you to build private services or public applications on the Zoom App Marketplace.
	/// </summary>
	/// <remarks>
	/// See Zoom documentation <a href="https://developers.zoom.us/docs/api/marketplace/">here</a> for more information.
	/// </remarks>
	public interface IMarketplace
	{
		/// <summary>
		/// Retrieve all of a user's app requests.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="requestType">The type of app request to retrieve.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="AppInfo">apps</see>.</returns>
		/// <remarks>
		/// Please note that this API returns summary information about the apps.
		/// For instance, it doesn't include the requirements, scopes or permissions for the apps.
		/// If you want detailed info, you need to invoke the <see cref="GetAppInfoAsync(string, CancellationToken)"/> method.
		/// </remarks>
		Task<PaginatedResponseWithToken<AppInfo>> GetUserAppRequestsAsync(string userId, AppRequestType requestType, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a user's entitlements.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of entitlement Ids.</returns>
		Task<long[]> GetUserEntitlementsAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve an app's user requests and the status of their approval.
		/// </summary>
		/// <param name="appId">The app Id.</param>
		/// <param name="status">The status of the app request.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="AppInfo">apps</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<AppRequest>> GetAppRequestsAsync(string appId, AppRequestStatus status, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all public apps.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="AppInfo">apps</see>.</returns>
		/// <remarks>
		/// Please note that this API returns summary information about the apps.
		/// For instance, it doesn't include the requirements, scopes or permissions for the apps.
		/// If you want detailed info, you need to invoke the <see cref="GetAppInfoAsync(string, CancellationToken)"/> method.
		/// </remarks>
		Task<PaginatedResponseWithToken<AppInfo>> GetPublicAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all apps created by this account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="AppInfo">apps</see>.</returns>
		/// <remarks>
		/// Please note that this API returns summary information about the apps.
		/// For instance, it doesn't include the requirements, scopes or permissions for the apps.
		/// If you want detailed info, you need to invoke the <see cref="GetAppInfoAsync(string, CancellationToken)"/> method.
		/// </remarks>
		Task<PaginatedResponseWithToken<AppInfo>> GetAccountAppsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create an event subscription.
		/// </summary>
		/// <param name="events">The events.</param>
		/// <param name="subscriptionName">The name of the event subsciption to be created.</param>
		/// <param name="webhookUrl">The event notification endpoint URL.</param>
		/// <param name="userIds">The usr Ids.</param>
		/// <param name="subscriptionScope">The app event subscription's scope.</param>
		/// <param name="accountId">The account Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The event subscription Id.</returns>
		/// <remarks>If the events parameter contains invalid event names, the Zoom API will return a HTTP 404 response.</remarks>
		Task<string> CreateEventSubscriptionAsync(IEnumerable<string> events, string subscriptionName, Uri webhookUrl, IEnumerable<string> userIds, string subscriptionScope, string accountId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get app information.
		/// </summary>
		/// <param name="appId">The app's ID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="AppInfo">app information</see>.</returns>
		Task<AppInfo> GetAppInfoAsync(string appId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add app allow requests for users​.
		/// </summary>
		/// <param name="appId">The app's ID.</param>
		/// <param name="userIds">The user Ids.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task AddAllowRequestForUsers(string appId, IEnumerable<string> userIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Add app allow requests for groups​.
		/// </summary>
		/// <param name="appId">The app's ID.</param>
		/// <param name="groupIds">The group Ids.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task AddAllowRequestForGroups(string appId, IEnumerable<string> groupIds, CancellationToken cancellationToken = default);
	}
}
