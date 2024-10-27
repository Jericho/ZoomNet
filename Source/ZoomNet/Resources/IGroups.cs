using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Resources;

/// <summary>
/// Allows you to manage Groups.
/// </summary>
public interface IGroups
{
	/// <summary>
	/// Adds users to a group.
	/// </summary>
	/// <param name="groupId">The ID of the group.</param>
	/// <param name="emails">An enumeration of email addresses of users to add to the group.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task representing the operation. The result will be an array of strings representing the IDs of the added users.</returns>
	public Task<string[]> AddUsersToGroupAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default);
}
