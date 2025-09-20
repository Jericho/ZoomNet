using Pathoschild.Http.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class ContactCenter : IContactCenter
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContactCenter" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal ContactCenter(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterUserRole>> GetAllRolesAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/roles")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterUserRole>("roles");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterUser>> SearchUsersAsync(string keyword, string regionId = null, UserStatus? status = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/users")
				.WithArgument("search_key", keyword)
				.WithArgument("region_id", regionId)
				.WithArgument("user_access", status?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterUser>("users");
		}

		/// <inheritdoc/>
		public Task<ContactCenterUser> GetUserAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/users/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterUser>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterUser> CreateUserAsync(string email, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "user_email", email },
			};

			return _client
				.PostAsync("contact_center/users")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterUser>();
		}
	}
}
