using Pathoschild.Http.Client;
using System;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Models.CallHandlingSettings;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Phone : IPhone
	{
		#region private fields

		private readonly IClient _client;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Phone(IClient client)
		{
			_client = client;
		}

		#endregion

		#region Recordings endpoints

		/// <inheritdoc/>
		public Task<PhoneCallRecording> GetRecordingAsync(
			string callId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_logs/{callId}/recordings")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecording>();
		}

		/// <inheritdoc/>
		public Task<PhoneCallRecordingTranscript> GetRecordingTranscriptAsync(
			string recordingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/recording_transcript/download/{recordingId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecordingTranscript>();
		}

		#endregion

		#region Users Endpoints

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
			int pageSize = 30,
			string nextPageToken = null,
			string siteId = null,
			int? callingType = null,
			PhoneCallUserStatus? status = null,
			string department = null,
			string costCenter = null,
			string keyword = null,
			CancellationToken cancellationToken = default)
		{
			if (pageSize < 1 || pageSize > 100)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Records per page must be between 1 and 100");
			}

			return _client
				.GetAsync($"phone/users")
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", nextPageToken)
				.WithArgument("site_id", siteId)
				.WithArgument("calling_type", callingType)
				.WithArgument("status", status)
				.WithArgument("department", department)
				.WithArgument("cost_center", costCenter)
				.WithArgument("keyword", keyword)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<PhoneUser>("users");
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
				.PatchAsync($"/v2/phone/extension/{extensionId}/call_handling/settings/{settingType.ToEnumString()}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		#endregion
	}
}
