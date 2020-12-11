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
		/// Adds users to a group.
		/// </summary>
		/// <param name="groupId">The ID of the group.</param>
		/// <param name="emails">An enumeration of email addresses of users to add to the group.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task representing the operation. The result will be an array of strings representing the IDs of the added users.</returns>
		Task<string[]> AddUsersToGroupAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve all groups on your account.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Group">groups</see>.
		/// </returns>
		Task<Group[]> GetAllAsync(CancellationToken cancellationToken = default);
	}
}
