using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
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
			.AsMessage()
			.ConfigureAwait(false);

		if (!response.IsSuccessStatusCode)
		{
			return Array.Empty<string>();
		}

		var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

		if (JsonNode.Parse(jsonResponse) is JsonObject jsonObject && jsonObject["ids"] is JsonValue idsValue)
		{
			return idsValue.ToString().Split(',');
		}

		return Array.Empty<string>();
	}
}

