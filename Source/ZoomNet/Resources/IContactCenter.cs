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
		Task<PaginatedResponseWithToken<ContactCenterUser>> SearchUserProfilesAsync(string keyword, string regionId = null, UserStatus? status = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a user's profile information.
		/// </summary>
		/// <param name="userId">The user Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The user.</returns>
		Task<User> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a user.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new user.
		/// </returns>
		Task<User> CreateUserAsync(string email, CancellationToken cancellationToken = default);
	}
}
