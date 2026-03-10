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

		#region Address Book

		/// <inheritdoc/>
		public Task<ContactCenterAddressBook> CreateAddressBookAsync(string name = null, string description = null, string unitId = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "address_book_name", name },
				{ "address_book_description", description },
				{ "unit_id", unitId }
			};

			return _client
				.PostAsync("contact_center/address_books")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBook>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBookContact> CreateAddressBookContactAsync(string addressbookId, string displayName, string firstName = null, string lastName = null, IEnumerable<(string Number, ContactCenterAddressBookPhoneNumberType Type)> phoneNumbers = null, IEnumerable<string> emails = null, string location = null, string timezone = null, string accountNumber = null, string company = null, string role = null, IEnumerable<(string Id, string Value)> variables = null, IEnumerable<string> consumerIds = null, IEnumerable<(string Id, string Value)> customFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "display_name", displayName },
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "phones", phoneNumbers?.Select(p => new JsonObject { { "phone_number", p.Number }, { "phone_type", p.Type.ToEnumString() } }).ToArray() },
				{ "emails", emails?.ToArray() },
				{ "location", location },
				{ "timezone", timezone },
				{ "account_number", accountNumber },
				{ "company", company },
				{ "role", role },
				{ "variables", variables?.Select(v => new JsonObject { { "variable_id", v.Id }, { "variable_value", v.Value } }).ToArray() },
				{ "consumer_ids", consumerIds?.ToArray() },
				{ "custom_fields", customFields?.Select(f => new JsonObject { { "custom_field_id", f.Id }, { "custom_field_value", f.Value } }).ToArray() }
			};

			return _client
				.PostAsync($"contact_center/address_books/{addressbookId}/contacts")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBookContact>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBookCustomField> CreateAddressBookCustomFieldAsync(string name, ContactCenterAddressBookCustomFieldDataType dataType, string description = null, string defaultValue = null, IEnumerable<string> pickListValues = null, IEnumerable<string> addressBookIds = null, bool? useAsRoutingProfileParameter = null, bool? useAsExternalUrlParameter = null, bool? showInTransferredCalls = null, bool? showInInboundNotification = null, bool? showInProfileTab = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "custom_field_name", name },
				{ "data_type", dataType.ToEnumString() },
				{ "custom_field_description", description },
				{ "default_value", defaultValue },
				{ "pick_list_values", pickListValues?.ToArray() },
				{ "address_book_ids", addressBookIds?.ToArray() },
				{ "use_as_routing_profile_parameter", useAsRoutingProfileParameter },
				{ "use_as_external_url_parameter", useAsExternalUrlParameter },
				{ "show_in_transferred_calls", showInTransferredCalls },
				{ "show_in_inbound_notification", showInInboundNotification },
				{ "show_in_profile_tab", showInProfileTab }
			};

			return _client
				.PostAsync("contact_center/address_books/custom_fields")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBookCustomField>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBookUnit> CreateAddressBookUnitAsync(string name = null, string description = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "unit_name", name },
				{ "unit_description", description }
			};

			return _client
				.PostAsync("contact_center/address_books/units")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBookUnit>();
		}

		/// <inheritdoc/>
		public Task DeleteAddressBookAsync(string addressBookId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/address_books/{addressBookId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAddressBookContactAsync(string addressBookId, string contactId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/address_books/{addressBookId}/contacts/{contactId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAddressBookCustomFieldAsync(string customFieldId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/address_books/custom_fields/{customFieldId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task DeleteAddressBookUnitAsync(string unitId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/address_books/units/{unitId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBook> GetAddressBookAsync(string addressBookId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/address_books/{addressBookId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBook>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBook> GetAddressBookContactAsync(string addressBookId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/address_books/{addressBookId}/contacts")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBook>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBookUnit> GetAddressBookUnitAsync(string addressBookUnitId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/address_books/units/{addressBookUnitId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBookUnit>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterAddressBookCustomField> GetAddressBookCustomFieldAsync(string customFieldId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/address_books/custom_fields/{customFieldId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBookCustomField>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterContactCustomField>> GetAllContactCustomFieldsAsync(string contactId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/address_books/contacts/{contactId}/custom_fields")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterContactCustomField>("custom_fields");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterAddressBookCustomField>> GetAllAddressBookContactsAsync(string addressBookId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"contact_center/address_books/{addressBookId}/contacts")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterAddressBookCustomField>("contacts");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterAddressBookUnit>> GetAllAddressBookUnitsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/address_books/units")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterAddressBookUnit>("units");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterAddressBook>> GetAllAddressBooksAsync(string unitId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/address_books")
				.WithArgument("unit_id", unitId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterAddressBook>("address_books");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterAddressBookCustomField>> GetAllAddressBookCustomFieldsAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/address_books/custom_fields")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterAddressBookCustomField>("custom_fields");
		}

		/// <inheritdoc/>
		public Task UpdateAddressBookAsync(string addressBookId, string name = null, string description = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "address_book_name", name },
				{ "address_book_description", description },
			};

			return _client
				.PatchAsync($"contact_center/address_books/{addressBookId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateAddressBookContactAsync(string contactId, string addressBookId, string displayName = null, string firstName = null, string lastName = null, IEnumerable<(string Number, ContactCenterAddressBookPhoneNumberType Type)> phoneNumbers = null, IEnumerable<string> emails = null, string location = null, string timezone = null, string accountNumber = null, string company = null, string role = null, IEnumerable<(string Id, string Value)> variables = null, IEnumerable<string> consumerIds = null, IEnumerable<(string Id, string Value)> customFields = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "display_name", displayName },
				{ "first_name", firstName },
				{ "last_name", lastName },
				{ "phones", phoneNumbers?.Select(p => new JsonObject { { "phone_number", p.Number }, { "phone_type", p.Type.ToEnumString() } }).ToArray() },
				{ "emails", emails?.ToArray() },
				{ "location", location },
				{ "timezone", timezone },
				{ "account_number", accountNumber },
				{ "company", company },
				{ "role", role },
				{ "variables", variables?.Select(v => new JsonObject { { "variable_id", v.Id }, { "variable_value", v.Value } }).ToArray() },
				{ "consumer_ids", consumerIds?.ToArray() },
				{ "custom_fields", customFields?.Select(f => new JsonObject { { "custom_field_id", f.Id }, { "custom_field_value", f.Value } }).ToArray() }
			};

			return _client
				.PatchAsync($"contact_center/address_books/{addressBookId}/contacts/{contactId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterAddressBookContact>();
		}

		/// <inheritdoc/>
		public Task UpdateAddressBookCustomFieldAsync(string customFieldId, string name = null, ContactCenterAddressBookCustomFieldDataType? dataType = null, string description = null, string defaultValue = null, IEnumerable<string> pickListValues = null, IEnumerable<string> addressBookIds = null, bool? useAsRoutingProfileParameter = null, bool? useAsExternalUrlParameter = null, bool? showInTransferredCalls = null, bool? showInInboundNotification = null, bool? showInProfileTab = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "custom_field_name", name },
				{ "data_type", dataType?.ToEnumString() },
				{ "custom_field_description", description },
				{ "default_value", defaultValue },
				{ "pick_list_values", pickListValues?.ToArray() },
				{ "address_book_ids", addressBookIds?.ToArray() },
				{ "use_as_routing_profile_parameter", useAsRoutingProfileParameter },
				{ "use_as_external_url_parameter", useAsExternalUrlParameter },
				{ "show_in_transferred_calls", showInTransferredCalls },
				{ "show_in_inbound_notification", showInInboundNotification },
				{ "show_in_profile_tab", showInProfileTab }
			};

			return _client
				.PatchAsync($"contact_center/address_books/custom_fields/{customFieldId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task UpdateAddressBookUnitAsync(string unitId, string name = null, string description = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "unit_name", name },
				{ "unit_description", description },
			};

			return _client
				.PatchAsync($"contact_center/address_books/units/{unitId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion

		#region Agent Statuses

		/// <inheritdoc/>
		public Task<ContactCenterSystemStatus> CreateAgentNotReadyReasonAsync(string name, IEnumerable<string> queues, bool enabled = true, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "enable", enabled },
				{ "queues", queues?.Select(id => new JsonObject { { "id", id } }).ToArray() },
				{ "status_category", ContactCenterAgentStatusCategory.NotReadyReason.ToEnumString() },
				{ "status_name", name },
			};

			return _client
				.PostAsync("contact_center/system_statuses")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSystemStatus>();
		}

		/// <inheritdoc/>
		public Task DeleteAgentStatusAsync(string statusId, CancellationToken cancellationToken = default)
		{
			return _client
				.DeleteAsync($"contact_center/system_statuses/{statusId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <inheritdoc/>
		public Task<ContactCenterSystemStatus> GetAgentStatusAsync(string statusId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"contact_center/system_statuses/{statusId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSystemStatus>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<ContactCenterSystemStatus>> GetAllAgentStatusesAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync("contact_center/system_statuses")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ContactCenterSystemStatus>("statuses");
		}

		/// <inheritdoc/>
		public Task<ContactCenterSystemStatus> UpdateAgentStatusAsync(string statusId, string name = null, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "status_category", ContactCenterAgentStatusCategory.SystemStatus.ToEnumString() },
				{ "system_status_name", name },
			};

			return _client
				.PatchAsync($"contact_center/system_statuses/{statusId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSystemStatus>();
		}

		/// <inheritdoc/>
		public Task<ContactCenterSystemStatus> UpdateAgentNotReadyReasonAsync(string statusId, string name, IEnumerable<string> queues, CancellationToken cancellationToken = default)
		{
			var data = new JsonObject
			{
				{ "enable", "true" },
				{ "queues", queues?.Select(id => new JsonObject { { "id", id } }).ToArray() },
				{ "status_category", ContactCenterAgentStatusCategory.NotReadyReason.ToEnumString() },
				{ "status_name", name },
			};

			return _client
				.PatchAsync($"contact_center/system_statuses/{statusId}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsObject<ContactCenterSystemStatus>();
		}

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
						{
							"multi_channel_engagement", new JsonObject
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
