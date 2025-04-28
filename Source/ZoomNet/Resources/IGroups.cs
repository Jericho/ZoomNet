using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage groups under an account.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/groups/groups">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IGroups
	{
		/// <summary>
		/// Retrieve all groups on your account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="Group">groups</see>.</returns>
		Task<Group[]> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a group on your account.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="Group">group</see>.</returns>
		Task<Group> GetAsync(string groupId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Create a group.
		/// </summary>
		/// <param name="name">The name of the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The newly created <see cref="Group">group</see>.</returns>
		Task<Group> CreateAsync(string name, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a group.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteAsync(string groupId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds members to a group.
		/// </summary>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="emails">An enumeration of email addresses of users to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of strings representing the member IDs of the added users.</returns>
		Task<string[]> AddMembersByEmailAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds members to a group.
		/// </summary>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="userIds">An enumeration of user IDs to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of strings representing the member IDs of the added users.</returns>
		Task<string[]> AddMembersByIdAsync(string groupId, IEnumerable<string> userIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds administrators to a group.
		/// </summary>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="emails">An enumeration of email addresses of users to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of strings representing the administrator IDs of the added users.</returns>
		Task<string[]> AddAdministratorsByEmailAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds administrators to a group.
		/// </summary>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="userIds">An enumeration of user IDs to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of strings representing the IDs of the added users.</returns>
		Task<string[]> AddAdministratorsByIdAsync(string groupId, IEnumerable<string> userIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove an administrator from a group.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="userId">The unique identifier of the administrator.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task RemoveAdministratorAsync(string groupId, string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove a member from a group.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="memberId">The unique identifier of the member.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task RemoveMemberAsync(string groupId, string memberId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the administrators associated with a group.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="GroupAdministrator">administrators</see>.</returns>
		Task<PaginatedResponseWithToken<GroupAdministrator>> GetAdministratorsAsync(string groupId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the members associated with a group.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="GroupMember">members</see>.</returns>
		Task<PaginatedResponseWithToken<GroupMember>> GetMembersAsync(string groupId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Update a group.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="name">The group name.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task UpdateAsync(string groupId, string name, CancellationToken cancellationToken = default);

		/// <summary>
		/// Upload a virtual background for all users in a group to use.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="pictureData">The binary data.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The newly created <see cref="VirtualBackgroundFile"/>.</returns>
		Task<VirtualBackgroundFile> UploadVirtualBackgroundAsync(string groupId, string fileName, Stream pictureData, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a group's Virtual Background files.
		/// </summary>
		/// <param name="groupId">The group unique identifier.</param>
		/// <param name="fileIds">An enumeration of file unique identifiers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The async task.</returns>
		Task DeleteVirtualBackgroundsAsync(string groupId, IEnumerable<string> fileIds, CancellationToken cancellationToken = default);
	}
}
