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

		/// <inheritdoc/>
		public Task AssignUsersToRegionAsync(IEnumerable<string> userIds, string regionId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "user_ids", userIds?.ToArray() },
			};

			return _client
				.PostAsync($"contact_center/regions/{regionId}/users")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterRegion> CreateRegionAsync(string name, string sipZoneId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "region_name", name },
				{ "sip_zone_id", sipZoneId },
			};

			return _client
				.PostAsync("contact_center/regions")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterRegion>();
		}

		/// <inheritdoc/>
		public Task DeleteRegionAsync(string regionId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/regions/{regionId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterRegion> GetRegionAsync(string regionId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/regions/{regionId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterRegion>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterUser>> GetRegionUsersAsync(string regionId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/regions/{regionId}/users")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterUser>("users");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterRegion>> GetAllRegionsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/regions")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterRegion>("regions");
		}

		/// <inheritdoc/>
		public Task UpdateRegionAsync(string regionId, string name = null, string sipZoneId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "region_name", name },
				{ "sip_zone_id", sipZoneId },
			};

			return _client
				.PatchAsync($"contact_center/regions/{regionId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

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

		/// <inheritdoc/>
		public Task<ContactCenterSkill> CreateSkillAsync(string name, string categoryId, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "skill_name", name },
				{ "skill_category_id", categoryId },
			};

			return _client
				.PostAsync("contact_center/skills")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSkill>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterSkillCategory> CreateSkillCategoryAsync(string name, string description = null, ContactCenterSkillType? type = null, int? maxProficiencyLevel = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "skill_category_name", name },
				{ "skill_category_description", description },
				{ "skill_type", type?.ToEnumString() },
				{ "max_proficiency_level", maxProficiencyLevel },
			};
			return _client
				.PostAsync("contact_center/skills/categories")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSkillCategory>();
		}

		/// <inheritdoc/>
		public Task DeleteSkillAsync(string skillId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/skills/{skillId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteSkillCategoryAsync(string skillCategoryId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/skills/categories/{skillCategoryId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterSkill> GetSkillAsync(string skillId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/skills/{skillId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSkill>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterSkillCategory> GetSkillCategoryAsync(string skillCategoryId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/skills/categories/{skillCategoryId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSkillCategory>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterSkill>> GetAllSkillsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/skills")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterSkill>("skills");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterSkillCategory>> GetAllSkillCategoriesAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/skills/categories")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterSkillCategory>("skill_categories");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterUser>> GetSkillUsersAsync(string skillId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/skills/{skillId}/users")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterUser>("users");
		}

		/// <inheritdoc/>
		public Task UpdateSkillAsync(string skillId, string name, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "skill_name", name },
			};

			return _client
				.PatchAsync($"contact_center/skills/{skillId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateSkillCategoryAsync(string skillCategoryId, string name = null, string description = null, int? maxProficiencyLevel = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "skill_category_name", name },
				{ "skill_category_description", description },
				{ "max_proficiency_level", maxProficiencyLevel },
			};

			return _client
				.PatchAsync($"contact_center/skills/categories/{skillCategoryId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region SMS

		#endregion

		#region Users

		/// <inheritdoc/>
		public Task AssignSkillsAsync(string userId, IEnumerable<(string SkillId, int ProficiencyLevel)> skills, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				// Confusingly, "max_proficiency_level" is intended to contain the user's proficiency level, not the 'max'.
				{ "skills", skills?.Select(s => new JsonObject { { "skill_id", s.SkillId }, { "max_proficiency_level", s.ProficiencyLevel } }).ToArray() },
			};

			return _client
				.PostAsync($"contact_center/users/{userId}/skills")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
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
			ContactCenterUserStatus? status = null,
			ContactCenterUserSubStatus? subStatus = null,
			ContactCenterUserAccessStatus? accessStatus = null,
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
				{ "status_id", (int?)status },
				{ "sub_status_id", (int?)subStatus },
				{ "status", accessStatus?.ToEnumString() },
			};

			return _client
				.PostAsync("contact_center/users")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterUser>();
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
		public Task<PaginatedResponseWithToken<ContactCenterQueue>> GetUserQueuesAsync(string userId, ContactCenterQueueChannel? channel = null, ContactCenterQueueAssignmentType assignementType = ContactCenterQueueAssignmentType.Agent, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/users/{userId}/queues")
				.WithArgument("channel", channel?.ToEnumString())
				.WithArgument("user_assignment_type", assignementType.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterQueue>("queues");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterUserSkill>> GetUserSkillsAsync(string userId, string categoryId = null, ContactCenterSkillType? skillType = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/users/{userId}/skills")
				.WithArgument("skill_category_id", categoryId)
				.WithArgument("skill_type", skillType?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterUserSkill>("skills");
		}

		/// <inheritdoc/>
		public Task UnassignSkillAsync(string userId, string skillId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/users/{userId}/skills/{skillId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterUser>> SearchUsersAsync(string keyword, string regionId = null, ContactCenterUserAccessStatus? accessStatus = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/users")
				.WithArgument("search_key", keyword)
				.WithArgument("region_id", regionId)
				.WithArgument("user_access", accessStatus?.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterUser>("users");
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
		public Task DeleteUsersAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync("contact_center/users")
				.WithArgument("user_ids", string.Join(",", userIds ?? Enumerable.Empty<string>()))
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
