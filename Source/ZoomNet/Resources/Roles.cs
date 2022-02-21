using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage roles.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IUsers" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/roles">Zoom documentation</a> for more information.
	/// </remarks>
	public class Roles : IRoles
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Roles" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Roles(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve all roles on your account.
		/// </summary>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Role">users</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<Role>> GetAllAsync(int recordsPerPage = 300, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"roles")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Role>("roles");
		}

		/// <summary>
		/// Creates a role.
		/// </summary>
		/// <param name="name">The role name.</param>
		/// <param name="description">Role description.</param>
		/// <param name="privileges">Array of assigned access rights as defined <see href="https://marketplace.zoom.us/docs/api-reference/other-references/privileges">HERE</see>.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The new role.
		/// </returns>
		public async Task<Role> CreateAsync(string name, string description = null, IEnumerable<string> privileges = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "name", name },
				{ "description", description },
				{ "privileges", privileges }
			};

			var result = await _client
				.PostAsync($"roles")
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

		/// <summary>
		/// Retrieve all users assigned to the specified role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="recordsPerPage">The number of records returned within a single API call.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Role">users</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<User>> GetMembersAsync(string roleId, int recordsPerPage = 300, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"roles/{roleId}/members")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<User>("members");
		}

		/// <summary>
		/// Assign users to a role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="userIds">A list of user ids or email addresses.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task AssignUsersAsync(string roleId, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
		{
			if (userIds == null || !userIds.Any()) throw new ArgumentNullException(nameof(userIds), "You must provide at least one user Id or email address.");

			var data = new JsonObject
			{
				// Zoom requires either the "id" field or the "email" field.
				// If both are provided, "id" takes precedence.
				{ "members", userIds.Select(id => new JsonObject { { (id ?? string.Empty).Contains("@") ? "email" : "id", id } }) }
			};

			return _client
				.PostAsync($"roles/{roleId}/members")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Remove user from a role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="userId">The user id or email address.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UnassignUserAsync(string roleId, string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"roles/{roleId}/members/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve the information of a specific role on a Zoom account.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Role" />.
		/// </returns>
		public Task<Role> GetAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"roles/{roleId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Role>();
		}

		/// <summary>
		/// Updates an existing role.
		/// </summary>
		/// <param name="id">The role ID.</param>
		/// <param name="name">The role name.</param>
		/// <param name="description">Role description.</param>
		/// <param name="privileges">Array of assigned access rights as defined <see href="https://marketplace.zoom.us/docs/api-reference/other-references/privileges">HERE</see>.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task UpdateRole(string id, string name = null, string description = null, IEnumerable<string> privileges = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "id", id },
				{ "name", name },
				{ "description", description },
				{ "privileges", privileges }
			};

			return _client
				.PatchAsync($"roles/{id}")
				.WithJsonBody(data)
				.WithHttp200TreatedAsFailure("The account must be a paid account to update the role.")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Delete a role.
		/// </summary>
		/// <param name="roleId">The role Id.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteAsync(string roleId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"roles/{roleId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
