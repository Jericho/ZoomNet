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
		#region Address Book

		#endregion

		#region Agent Statuses

		#endregion

		#region Asset Library

		#endregion

		#region Call Control

		#endregion

		#region Campaigns

		#endregion

		#region Dispositions

		#endregion

		#region Engagements

		#endregion

		#region Flows

		#endregion

		#region Inboxes

		#endregion

		#region Logs

		#endregion

		#region Notes

		#endregion

		#region Operating Hours

		#endregion

		#region Queues

		/// <summary>
		/// Retrieves a paginated list of contact center queues, optionally filtered by channel.
		/// </summary>
		/// <param name="channel">The channel to filter queues by. If null, queues from all channels are included.</param>
		/// <param name="recordsPerPage">The maximum number of queues to return in a single page. Must be a positive integer.</param>
		/// <param name="pagingToken">A token indicating the starting point for pagination. Pass null or an empty string to retrieve the first page.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with a collection
		/// of contact center queues and a token for retrieving the next page, if available.</returns>
		Task<PaginatedResponseWithToken<ContactCenterQueue>> GetAllQueuesAsync(ContactCenterQueueChannel? channel = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously deletes the queue identified by the specified queue ID.
		/// </summary>
		/// <param name="queueId">The unique identifier of the queue to delete. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteQueueAsync(string queueId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a paginated list of agents assigned to the specified contact center queue asynchronously.
		/// </summary>
		/// <param name="queueId">The unique identifier of the contact center queue for which agents are to be retrieved. Cannot be null or empty.</param>
		/// <param name="recordsPerPage">The maximum number of agents to include in each page of results. Must be a positive integer. The default is 30.</param>
		/// <param name="pagingToken">An optional token indicating the starting point for pagination. If null, retrieval begins from the first page.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with a collection
		/// of queue agents and a token for retrieving the next page, if available.</returns>
		Task<PaginatedResponseWithToken<ContactCenterQueueAgent>> GetAllQueueAgentsAsync(string queueId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously removes the specified agent from the given queue.
		/// </summary>
		/// <remarks>If the agent is not currently assigned to the specified queue, the operation completes without
		/// error. This method does not throw if the agent or queue does not exist, but no changes will be made in such
		/// cases.</remarks>
		/// <param name="queueId">The unique identifier of the queue from which the agent will be unassigned. Cannot be null or empty.</param>
		/// <param name="userId">The unique identifier of the agent to unassign from the queue. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the unassignment operation.</param>
		/// <returns>A task that represents the asynchronous unassignment operation.</returns>
		Task UnassignAgentAsync(string queueId, string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously creates a new contact center queue with the specified name, description, and channel.
		/// </summary>
		/// <param name="name">The unique name of the queue to create. Cannot be null or empty.</param>
		/// <param name="description">An optional description for the queue. If null, the queue will be created without a description.</param>
		/// <param name="channel">The channel type for the queue, such as voice or chat.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created <see
		/// cref="ContactCenterQueue"/> instance.</returns>
		Task<ContactCenterQueue> CreateQueueAsync(string name, string description = null, ContactCenterQueueChannel channel = ContactCenterQueueChannel.Voice, CancellationToken cancellationToken = default);

		/// <summary>
		/// Assigns the specified agents to the given queue asynchronously.
		/// </summary>
		/// <remarks>If any user ID in <paramref name="userIds"/> is already assigned to the queue, it will not be
		/// reassigned. This method does not remove existing agent assignments from the queue.</remarks>
		/// <param name="queueId">The unique identifier of the queue to which agents will be assigned. Cannot be null or empty.</param>
		/// <param name="userIds">A collection of user IDs representing the agents to assign to the queue. Cannot contain null or empty values.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the assignment operation.</param>
		/// <returns>A task that represents the asynchronous assignment operation.</returns>
		Task AssignAgentsAsync(string queueId, IEnumerable<string> userIds, CancellationToken cancellationToken = default);

		#endregion

		#region Recordings

		#endregion

		#region Regions

		#endregion

		#region Reports v2 (CX analytics)

		#endregion

		#region Reports (Legacy Reports)

		#endregion

		#region Roles

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
		/// Asynchronously deletes the role identified by the specified role ID.
		/// </summary>
		/// <param name="roleId">The unique identifier of the role to delete. Cannot be null or empty.</param>
		/// <param name="transferRoleId">The unique identifier of an existing role to which users assigned to the deleted role will be transferred. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteRoleAsync(string roleId, string transferRoleId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a new contact center user role with the specified name, description, and set of privileges asynchronously.
		/// </summary>
		/// <param name="name">The name of the role to create. Cannot be null or empty.</param>
		/// <param name="description">The description of the role. Provides additional context for the role's purpose.</param>
		/// <param name="privileges">A collection of privilege identifiers to assign to the role. Each privilege defines an action or permission
		/// granted to users with this role.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created ContactCenterUserRole
		/// instance.</returns>
		/// <remarks>The full list of proivileges is available <a href="https://developers.zoom.us/docs/api/references/contact-center-privileges/">here</a>.</remarks>
		Task<ContactCenterUserRole> CreateRoleAsync(string name, string description, IEnumerable<string> privileges, CancellationToken cancellationToken = default);

		#endregion

		#region Routing Profiles

		#endregion

		#region Skills

		#endregion

		#region SMS

		#endregion

		#region Users

		/// <summary>
		/// Asynchronously assigns the specified skills to the user identified by the given user ID.
		/// </summary>
		/// <param name="userId">The unique identifier of the user to whom the skills will be assigned. Cannot be null or empty.</param>
		/// <param name="skills">A collection of skill names to assign to the user. Cannot be null. Each skill name should be a non-empty string.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task AssignSkillsAsync(string userId, IEnumerable<string> skills, CancellationToken cancellationToken = default);

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
		/// <param name="status">The status to assign to the user. Optional.</param>
		/// <param name="subStatus">The user's reason when the user status is 'Not Ready'. Optional.</param>
		/// <param name="accessStatus">The use access status. Either active or inactive. Optional.</param>
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
			ContactCenterUserStatus? status = null,
			ContactCenterUserSubStatus? subStatus = null,
			ContactCenterUserAccessStatus? accessStatus = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's profile information.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The user.</returns>
		Task<ContactCenterUser> GetUserAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously retrieves a paginated list of queues assigned to the specified user, optionally filtered by channel
		/// and assignment type.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose queue assignments are to be retrieved. Cannot be null or empty.</param>
		/// <param name="channel">An optional value specifying the channel to filter queues by. If null, queues from all channels are included.</param>
		/// <param name="assignementType">The type of queue assignment to filter by, such as Agent or Supervisor. Defaults to Agent.</param>
		/// <param name="recordsPerPage">The maximum number of records to include in each page of results. Must be greater than zero. Defaults to 30.</param>
		/// <param name="pagingToken">An optional token indicating the starting point for pagination. If null, retrieval begins from the first page.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with a collection
		/// of queue assignments for the specified user and a token for retrieving the next page, if available.</returns>
		Task<PaginatedResponseWithToken<ContactCenterQueue>> GetUserQueuesAsync(string userId, ContactCenterQueueChannel? channel = null, ContactCenterQueueAssignmentType assignementType = ContactCenterQueueAssignmentType.Agent, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves a paginated list of contact center skills assigned to the specified user, optionally filtered by
		/// category and skill type.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose skills are to be retrieved. Cannot be null or empty.</param>
		/// <param name="categoryId">The identifier of the skill category to filter results. If null, skills from all categories are included.</param>
		/// <param name="skillType">The type of skill to filter results. Defaults to proficiency-based skills.</param>
		/// <param name="recordsPerPage">The maximum number of skill records to include in each page of results. Must be greater than zero.</param>
		/// <param name="pagingToken">A token indicating the starting point for pagination. If null, retrieval begins from the first page.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Allows the operation to be cancelled.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a paginated response with a collection
		/// of contact center skills and a token for retrieving the next page, if available.</returns>
		Task<PaginatedResponseWithToken<ContactCenterUserSkill>> GetUserSkillsAsync(string userId, string categoryId = null, ContactCenterSkillType? skillType = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously unassigns the specified skills from the user identified by the given user ID.
		/// </summary>
		/// <param name="userId">The unique identifier of the user to whom the skills will be unassigned. Cannot be null or empty.</param>
		/// <param name="skillId">The unique identifier of the the skill to unassign from the user. Cannot be null.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task UnassignSkillAsync(string userId, string skillId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Search users and their information.
		/// </summary>
		/// <param name="keyword">The search keyword: either email address or username.</param>
		/// <param name="regionId">The region Id to filter results by.</param>
		/// <param name="accessStatus">The user's access status to filter results by.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Contact">contacts</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<ContactCenterUser>> SearchUsersAsync(string keyword, string regionId = null, ContactCenterUserAccessStatus? accessStatus = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously deletes the user identified by the specified user ID.
		/// </summary>
		/// <param name="userId">The unique identifier of the user to delete. Cannot be null or empty.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously deletes the specified users from the system.
		/// </summary>
		/// <remarks>If any user identifier does not correspond to an existing user, it will be ignored. The operation
		/// is performed atomically for all valid user identifiers.</remarks>
		/// <param name="userIds">A collection of user identifiers representing the users to delete. Cannot contain null or empty values.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
		/// <returns>A task that represents the asynchronous delete operation.</returns>
		Task DeleteUsersAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously updates the status and optional sub-status for the specified user.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose status will be updated. Cannot be null or empty.</param>
		/// <param name="status">The status to assign to the user.</param>
		/// <param name="subStatus">The user's reason when the user status is 'Not Ready'.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
		/// <returns>A task that represents the asynchronous update operation.</returns>
		Task UpdateUserStatusAsync(string userId, ContactCenterUserStatus status, ContactCenterUserSubStatus? subStatus = null, CancellationToken cancellationToken = default);

		#endregion

		#region Variables

		#endregion
	}
}
