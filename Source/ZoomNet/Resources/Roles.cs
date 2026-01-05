using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
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
	public class Roles : IRoles
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Roles" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Roles(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Role>> GetAllAsync(int recordsPerPage = 300, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("roles")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Role>("roles");
		}

		/// <inheritdoc/>
		public async Task<Role> CreateAsync(string name, string description = null, IEnumerable<string> privileges = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "privileges", privileges?.ToArray() }
			};

			var result = await _client
				.PostAsync("roles")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("You do not have the permission to create a role.")
				.WithCancellationToken(cancellationToken)
				.AsMessage()
				.ConfigureAwait(false);

			if (result.IsSuccessStatusCode)
			{
				return await result.Content.ReadAsAsync<Role>().ConfigureAwait(false);
			}
			else
			{
				throw new HttpRequestException($"Error creating role, Zoom returned {(int)result.StatusCode} status code.");
			}
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<User>> GetMembersAsync(string roleId, int recordsPerPage = 300, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"roles/{roleId}/members")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<User>("members");
		}

		/// <inheritdoc/>
		public Task AssignUsersAsync(string roleId, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNullOrEmpty(userIds, nameof(userIds), "You must provide at least one user Id or email address.");

			var data = new JsonObject
			{
				// Zoom requires either the "id" field or the "email" field.
				// If both are provided, "id" takes precedence.
				{ "members", userIds.Select(id => new JsonObject { { (id ?? string.Empty).Contains('@') ? "email" : "id", id } }).ToArray() }
			};

			return _client
				.PostAsync($"roles/{roleId}/members")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UnassignUserAsync(string roleId, string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"roles/{roleId}/members/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<Role> GetAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"roles/{roleId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Role>();
		}

		/// <inheritdoc/>
		public Task UpdateRole(string id, string name = null, string description = null, IEnumerable<string> privileges = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "id", id },
				{ "name", name },
				{ "description", description },
				{ "privileges", privileges?.ToArray() }
			};

			return _client
				.PatchAsync($"roles/{id}")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("The account must be a paid account to update the role.")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"roles/{roleId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
