using Pathoschild.Http.Client;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Resources;

/// <summary>
/// Allows you to manage Groups.
/// </summary>
public class Groups : IGroups
{
	private readonly IClient _client;

	internal Groups(IClient client)
	{
		_client = client;
	}

	/// <summary>
	/// Adds a user to a group.
	/// </summary>
	/// <param name="groupId">The ID of the group.</param>
	/// <param name="userEmail">The email of the user to add to the group.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task representing the operation. The result will be true if the user was added successfully, false otherwise.</returns>
	/// <inheritdoc/>
	public async Task<bool> AddUserToGroupAsync(string groupId, string userEmail, CancellationToken cancellationToken = default)
	{
		var data = new JsonObject
		{
			{ "members", new JsonArray(new JsonObject { { "email", userEmail } }) }
		};

		var response = await _client
			.PostAsync($"groups/{groupId}/members")
			.WithJsonBody(data)
			.WithCancellationToken(cancellationToken)
			.AsMessage()
			.ConfigureAwait(false);

		return response.IsSuccessStatusCode;
	}
}
