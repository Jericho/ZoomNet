using Pathoschild.Http.Client;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomNet.Resources;

/// <inheritdoc/>
public class Groups : IGroups
{
	private readonly IClient _client;

	internal Groups(IClient client)
	{
		_client = client;
	}

	/// <inheritdoc/>
	public async Task<string[]> AddUsersToGroupAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default)
	{
		var membersArray = new JsonArray();
		foreach (var email in emails)
		{
			membersArray.Add(new JsonObject { { "email", email } });
		}

		var data = new JsonObject
		{
			{ "members", membersArray },
		};

		var response = await _client
			.PostAsync($"groups/{groupId}/members")
			.WithJsonBody(data)
			.WithCancellationToken(cancellationToken)
			.AsJson()
			.ConfigureAwait(false);

		return response.GetPropertyValue("ids", string.Empty).Split(',');
	}
}
