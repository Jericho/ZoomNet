using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage roles.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/roles/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IRoles
	{
		/// <summary>
		/// Retrieve all roles on your account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Role">users</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<Role>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a role.
		/// </summary>
		/// <param name="name">The role name.</param>
		/// <param name="description">Role description.</param>
		/// <param name="privileges">Array of assigned access rights as defined <see href="https://marketplace.zoom.us/docs/api-reference/other-references/privileges">HERE</see>.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new role.
		/// </returns>
		Task<Role> CreateAsync(string name, string description = null, IEnumerable<string> privileges = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all users assigned to the specified role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Role">users</see>.
		/// </returns>
		Task<PaginatedResponseWithToken<User>> GetMembersAsync(string roleId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Assign users to a role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="userIds">A list of user ids or email addresses.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task AssignUsersAsync(string roleId, IEnumerable<string> userIds, CancellationToken cancellationToken = default);

		/// <summary>
		/// Remove user from a role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="userId">The user id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UnassignUserAsync(string roleId, string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve the information of a specific role on a Zoom account.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="User" />.
		/// </returns>
		Task<Role> GetAsync(string roleId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates an existing role.
		/// </summary>
		/// <param name="id">The role ID.</param>
		/// <param name="name">The role name.</param>
		/// <param name="description">Role description.</param>
		/// <param name="privileges">Array of assigned access rights as defined <see href="https://marketplace.zoom.us/docs/api-reference/other-references/privileges">HERE</see>.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task UpdateRole(string id, string name = null, string description = null, IEnumerable<string> privileges = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Delete a role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAsync(string roleId, CancellationToken cancellationToken = default);
	}
}
