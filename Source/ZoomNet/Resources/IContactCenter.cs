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
		/// Creates a user.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new user.
		/// </returns>
		Task<ContactCenterUser> CreateUserAsync(string email, CancellationToken cancellationToken = default);
	}
}
