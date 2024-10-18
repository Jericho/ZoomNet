using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Resources;

/// <summary>
/// Allows you to manage Groups.
/// </summary>
public interface IGroups
{
	/// <summary>
	/// Adds a user to a group.
	/// </summary>
	/// <param name="groupId">The ID of the group.</param>
	/// <param name="userEmail">The email of the user to add to the group.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task representing the operation. The result will be true if the user was added successfully, false otherwise.</returns>
	Task<bool> AddUserToGroupAsync(string groupId, string userEmail, CancellationToken cancellationToken = default);
}
