using Pathoschild.Http.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Groups : IGroups
	/// <summary>
	/// Initializes a new instance of the <see cref="Groups" /> class.
	/// </summary>
	/// <param name="client">The HTTP client.</param>
	internal Groups(IClient client)
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Groups" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Groups(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public async Task<string[]> AddUsersToGroupAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "members", emails?.Select(e => new JsonObject { { "email", e } }).ToArray() }
			};

			var response = await _client
				.PostAsync($"groups/{groupId}/members")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			return response.GetPropertyValue("ids", string.Empty).Split(',');
		}

		/// <inheritdoc/>
		public Task<Group[]> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"groups")
				.WithCancellationToken(cancellationToken)
				.AsObject<Group[]>("groups");
		}
	}
}
