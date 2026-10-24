using Pathoschild.Http.Client;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.CallHandlingSettings;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Phone : IPhone
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Phone(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PhoneCallRecording> GetRecordingAsync(string callId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_logs/{callId}/recordings")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecording>();
		}

		/// <inheritdoc/>
		public Task<PhoneCallRecordingTranscript> GetRecordingTranscriptAsync(string recordingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/recording_transcript/download/{recordingId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecordingTranscript>();
		}

		/// <inheritdoc/>
		public Task<PhoneCallUserProfile> GetPhoneCallUserProfileAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/users/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallUserProfile>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<PhoneUser>> ListPhoneUsersAsync(
			int recordsPerPage = 30,
			string nextPageToken = null,
			string siteId = null,
			int? callingType = null,
			PhoneCallUserStatus? status = null,
			string department = null,
			string costCenter = null,
			string keyword = null,
			CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 100);

			return _client
				.GetAsync("phone/users")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", nextPageToken)
				.WithArgument("site_id", siteId)
				.WithArgument("calling_type", callingType)
				.WithArgument("status", status?.ToEnumString())
				.WithArgument("department", department)
				.WithArgument("cost_center", costCenter)
				.WithArgument("keyword", keyword)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<PhoneUser>("users");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<PhoneDetail>> ListPhonesAsync(
			int recordsPerPage = 30,
			string nextPageToken = null,
			string siteId = null,
			PhoneNumberAssignmentType? assignmentType = null,
			PhoneNumberExtensionType? extensionType = null,
			PhoneNumberType? numberType = null,
			bool? pendingNumbers = null,
			string keyword = null,
			CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 100);

			return _client
				.GetAsync("phone/numbers")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", nextPageToken)
				.WithArgument("site_id", siteId)
				.WithArgument("type", assignmentType?.ToEnumString())
				.WithArgument("extension_type", extensionType?.ToEnumString())
				.WithArgument("number_type", numberType?.ToEnumString())
				.WithArgument("pending_numbers", pendingNumbers)
				.WithArgument("keyword", keyword)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<PhoneDetail>("phone_numbers");
		}

		/// <inheritdoc/>
		public Task UpdateCallHandlingSettingsAsync(
			string extensionId,
			CallHandlingSettingType settingType,
			CallHandlingSubsettingsBase subsettings,
			CancellationToken cancellationToken)
		{
			var data = new JsonObject
			{
				{ "settings", subsettings },
				{ "sub_setting_type", subsettings?.SubsettingType }
			};

			return _client
				.PatchAsync($"phone/extension/{extensionId}/call_handling/settings/{settingType.ToEnumString()}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#region Call Queues
		/// <inheritdoc/>
		public Task<CallQueue> GetCallQueueAsync(string callQueueId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_queues/{callQueueId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<CallQueue>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<CallQueue>> GetAllCallQueuesAsync(string department = null, string costCenter = null, string siteId = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 100);

			return _client
				.GetAsync($"phone/call_queues")
				.WithArgument("department", department)
				.WithArgument("cost_center", costCenter)
				.WithArgument("site_id", siteId)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<CallQueue>("call_queues");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<CallQueueMember>> GetCallQueueMembersAsync(string callQueueId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);
			return _client
				.GetAsync($"phone/call_queues/{callQueueId}/members")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<CallQueueMember>("call_queue_members");
		}
		#endregion
	}
}
