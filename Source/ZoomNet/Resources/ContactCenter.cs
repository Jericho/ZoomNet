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


		#region Agent Statuses

		#endregion

		#region Asset Library

		#endregion

		#region Call Control

		#endregion

		#region Campaigns

		#endregion

		#region Dispositions

		#endregion

		#region Engagements

		#endregion

		#region Flows

		#endregion

		#region Inboxes

		#endregion

		#region Logs

		#endregion

		#region Notes

		#endregion

		#region Operating Hours

		#endregion

		#region Queues

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterQueue>> GetAllQueuesAsync(ContactCenterQueueChannel? channel = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/queues")
				.WithArgument("channel", channel?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterQueue>("queues");
		}

		/// <inheritdoc/>
		public Task DeleteQueueAsync(string queueId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/queues/{queueId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterQueueAgent>> GetAllQueueAgentsAsync(string queueId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/queues/{queueId}/agents")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterQueueAgent>("agents");
		}

		/// <inheritdoc/>
		public Task UnassignAgentAsync(string queueId, string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/queues/{queueId}/agents/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterQueue> CreateQueueAsync(string name, string description = null, ContactCenterQueueChannel channel = ContactCenterQueueChannel.Video, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "queue_name", name },
				{ "queue_description", description },
				{ "channel", channel.ToEnumString() },
				{ "channel_types", new[] { channel.ToEnumString() } } // Documentation says "channel_types" is deprecated but the API throws an error if it's not used
			};

			return _client
				.PostAsync("contact_center/queues")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterQueue>();
		}

		/// <inheritdoc/>
		public Task AssignAgentsAsync(string queueId, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "user_ids", userIds?.ToArray() }
			};

			return _client
				.PostAsync($"contact_center/queues/{queueId}/agents")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region Recordings

		#endregion

		#region Regions

		#endregion

		#region Reports v2 (CX analytics)

		#endregion

		#region Reports (Legacy Reports)

		#endregion

		#region Roles

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
		public Task DeleteRoleAsync(string roleId, string transferRoleId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/roles/{roleId}")
				.WithArgument("transfer_role_id", transferRoleId)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterUserRole> CreateRoleAsync(string name, string descirption, IEnumerable<string> privileges, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "role_name", name },
				{ "role_description", descirption },
				{ "privileges", privileges?.ToArray() }
			};
			return _client
				.PostAsync("contact_center/roles")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterUserRole>();
		}

		#endregion

		#region Routing Profiles

		#endregion

		#region Skills

		#endregion

		#region SMS

		#endregion

		#region Users

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
		public Task<ContactCenterUser> CreateUserAsync(
			string email,
			string roleId,
			IEnumerable<string> addOnsPlan = null,
			int? maxConcurrentEmailConversations = null,
			int? maxConcurrentMessagingConversations = null,
			int? maxEmailLoadPercentage = null,
			bool enableVoiceAndVideoEngagement = true,
			int? maxLoadPercentage = null,
			string clientIntegration = null,
			string clientIntegrationName = null,
			string name = null,
			string packageName = null,
			string regionId = null,
			string statusId = null,
			string statusName = null,
			string subStatusId = null,
			string subStatusName = null,
			string status = null,
			CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "user_email", email },
				{ "role_id", roleId },
				{ "add_ons_plan", addOnsPlan?.ToArray() },
				{
					"channel_settings", new JsonObject
					{
						{ "concurrent_email_capacity", maxConcurrentEmailConversations },
						{ "concurrent_message_capacity", maxConcurrentMessagingConversations },
						{ "multi_channel_engagement", new JsonObject
							{
								{ "email_max_agent_load", maxEmailLoadPercentage },
								{ "enabled", enableVoiceAndVideoEngagement },
								{ "max_agent_load", maxLoadPercentage },
							}
						},
					}
				},
				{ "client_integration", clientIntegration },
				{ "client_integration_name", clientIntegrationName },
				{ "name", name },
				{ "package_name", packageName },
				{ "region_id", regionId },
				{ "status_id", statusId },
				{ "status_name", statusName },
				{ "sub_status_id", subStatusId },
				{ "sub_status_name", subStatusName },
				{ "status", status }
			};

			return _client
				.PostAsync("contact_center/users")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterUser>();
		}

		/// <inheritdoc/>
		public Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/users/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateUserStatusAsync(string userId, ContactCenterUserStatus status, ContactCenterUserSubStatus? subStatus = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "status_id", (int)status },
				{ "sub_status_id", (int)subStatus }
			};

			return _client
				.PatchAsync($"contact_center/users/{userId}/status")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region Variables

		#endregion
	}
}
