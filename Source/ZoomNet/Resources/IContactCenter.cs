using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage data on the Zoom Contact Center.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/contacts/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IContactCenter
	{
		/// <summary>
		/// Retrieves a paginated list of all contact center user roles asynchronously.
		/// </summary>
		/// <param name="recordsPerPage">The maximum number of roles to include in each page of results. Must be a positive integer. The default is 30.</param>
		/// <param name="pagingToken">An optional token indicating the starting point for the next page of results. Pass null or omit to retrieve the
		/// first page.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with a collection
		/// of contact center user roles and a token for retrieving the next page, if available.</returns>
		Task<PaginatedResponseWithToken<ContactCenterUserRole>> GetAllRolesAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Search users and their information.
		/// </summary>
		/// <param name="keyword">The search keyword: either email address or username.</param>
		/// <param name="regionId">The region Id to filter results by.</param>
		/// <param name="status">The user status to filter results by.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Contact">contacts</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ContactCenterUser>> SearchUsersAsync(string keyword, string regionId = null, UserStatus? status = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's profile information.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The user.</returns>
		Task<ContactCenterUser> GetUserAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a new contact center user with the specified configuration and returns the created user asynchronously.
		/// </summary>
		/// <param name="email">The email address of the user to create. Cannot be null or empty.</param>
		/// <param name="roleId">The identifier of the role to assign to the user. Cannot be null or empty.</param>
		/// <param name="addOnsPlan">A collection of add-on plan identifiers to associate with the user. Optional; pass null to assign no add-ons.</param>
		/// <param name="maxConcurrentEmailConversations">The maximum number of concurrent email conversations the user can handle. Optional; if not specified, the system
		/// default is used.</param>
		/// <param name="maxConcurrentMessagingConversations">The maximum number of concurrent messaging conversations the user can handle. Optional; if not specified, the
		/// system default is used.</param>
		/// <param name="maxEmailLoadPercentage">The maximum email load percentage allowed for the user. Optional; must be between 0 and 100 if specified.</param>
		/// <param name="enableVoiceAndVideoEngagement">A value indicating whether to allow the user to receive voice or video engagements while handling chat and SMS engagements, based on the max_agent_load value. The default is <see langword="true"/>.</param>
		/// <param name="maxLoadPercentage">The maximum overall load percentage allowed for the user. Optional; must be between 0 and 100 if specified.</param>
		/// <param name="clientIntegration">The identifier of the client integration to associate with the user. Optional.</param>
		/// <param name="clientIntegrationName">The name of the client integration to associate with the user. Optional.</param>
		/// <param name="name">The display name of the user. Optional.</param>
		/// <param name="packageName">The package name to assign to the user. Optional.</param>
		/// <param name="regionId">The identifier of the region in which to create the user. Optional.</param>
		/// <param name="statusId">The identifier of the user's status. Optional.</param>
		/// <param name="statusName">The name of the user's status. Optional.</param>
		/// <param name="subStatusId">The identifier of the user's sub-status. Optional.</param>
		/// <param name="subStatusName">The name of the user's sub-status. Optional.</param>
		/// <param name="status">The status value to assign to the user. Optional.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created <see
		/// cref="ContactCenterUser"/> instance.</returns>
		Task<ContactCenterUser> CreateUserAsync(
			string email,
			string roleId,
			IEnumerable<string> addOnsPlan = null,
			int? maxConcurrentEmailConversations = null,
			int? maxConcurrentMessagingConversations = null,
			int? maxEmailLoadPercentage = null,
			bool enableVoiceAndVideoEngagement = true,
			int? maxLoadPercentage = null,
			string clientIntegration = null,
			string clientIntegrationName = null,
			string name = null,
			string packageName = null,
			string regionId = null,
			string statusId = null,
			string statusName = null,
			string subStatusId = null,
			string subStatusName = null,
			string status = null,
			CancellationToken cancellationToken = default);
	}
}
