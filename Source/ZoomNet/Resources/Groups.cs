using Pathoschild.Http.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Groups : IGroups
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Groups" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Groups(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<Group[]> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"groups")
				.WithCancellationToken(cancellationToken)
				.AsObject<Group[]>("groups");
		}

		/// <inheritdoc/>
		public Task<Group> GetAsync(string groupId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"groups/{groupId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Group>();
		}

		/// <inheritdoc/>
		public Task<Group> CreateAsync(string name, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name }
			};

			return _client
				.PostAsync($"groups")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<Group>();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(string groupId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"groups/{groupId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public async Task<string[]> AddMembersByEmailAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default)
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
		public async Task<string[]> AddMembersByIdAsync(string groupId, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "members", userIds?.Select(e => new JsonObject { { "id", e } }).ToArray() }
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
		public async Task<string[]> AddAdministratorsByEmailAsync(string groupId, IEnumerable<string> emails, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "admins", emails?.Select(e => new JsonObject { { "email", e } }).ToArray() }
			};

			var response = await _client
				.PostAsync($"groups/{groupId}/admins")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			return response.GetPropertyValue("ids", string.Empty).Split(',');
		}

		/// <inheritdoc/>
		public async Task<string[]> AddAdministratorsByIdAsync(string groupId, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "admins", userIds?.Select(e => new JsonObject { { "id", e } }).ToArray() }
			};

			var response = await _client
				.PostAsync($"groups/{groupId}/admins")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsJson()
				.ConfigureAwait(false);

			return response.GetPropertyValue("ids", string.Empty).Split(',');
		}

		/// <inheritdoc/>
		public Task RemoveAdministratorAsync(string groupId, string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"groups/{groupId}/admins/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task RemoveMemberAsync(string groupId, string memberId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"groups/{groupId}/members/{memberId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<GroupAdministrator>> GetAdministratorsAsync(string groupId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"groups/{groupId}/admins")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<GroupAdministrator>("admins");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<GroupMember>> GetMembersAsync(string groupId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"groups/{groupId}/members")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<GroupMember>("members");
		}

		/// <inheritdoc/>
		public Task UpdateAsync(string groupId, string name, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name }
			};

			return _client
				.PatchAsync($"groups/{groupId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<VirtualBackgroundFile> UploadVirtualBackgroundAsync(string groupId, string fileName, Stream pictureData, CancellationToken cancellationToken = default)
		{
			return _client
				.PostAsync($"groups/{groupId}/settings/virtual_backgrounds")
				.WithBody(bodyBuilder =>
				{
					var content = new MultipartFormDataContent();

					// Zoom requires the 'name' to be 'file'. Also, you
					// must specify the 'fileName' otherwise Zoom will return
					// a very confusing HTTP 400 error with the following body:
					// {"code":-1,"message":"Required request part 'file' is not present"}
					content.Add(new StreamContent(pictureData), "file", fileName);

					return content;
				})
				.WithCancellationToken(cancellationToken)
				.AsObject<VirtualBackgroundFile>();
		}

		/// <inheritdoc/>
		public Task DeleteVirtualBackgroundsAsync(string groupId, IEnumerable<string> fileIds, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"groups/{groupId}/settings/virtual_backgrounds")
				.WithArgument("file_ids", string.Join(",", fileIds))
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
