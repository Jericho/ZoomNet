using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage users.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/users/users">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IUsers
	{
		/// <summary>
		/// Retrieve all users on your account.
		/// </summary>
		/// <param name="status">The user status. Allowed values: Active, Inactive, pending.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="page">The current page number of returned records.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Users">users</see>.
		/// </returns>
		Task<PaginatedResponse<User>> GetAllAsync(UserStatus status = UserStatus.Active, int recordsPerPage = 30, int page = 1, CancellationToken cancellationToken = default);
	}
}
