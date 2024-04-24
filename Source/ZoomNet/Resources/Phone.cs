using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc cref="IPhone" select="remarks"/>
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

		/// <inheritdoc cref="IPhone.GetRecordingAsync" select="remarks"/>
		public Task<PhoneCallRecording> GetRecordingAsync(
			string callId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_logs/{callId}/recordings")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallRecording>();
		}

		/// <inheritdoc cref="IPhone.GetRecordingTranscriptAsync" select="remarks"/>
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

		/// <inheritdoc cref="IPhone.GetPhoneCallUserProfileAsync" select="remarks" />
		public Task<PhoneCallUserProfile> GetPhoneCallUserProfileAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/users/{userId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PhoneCallUserProfile>();
		}

		/// <inheritdoc cref="IPhone.ListPhoneCallUserProfilesAsync" select="remarks" />
		public Task<PhoneCallUserProfilesPaginationObject> ListPhoneCallUserProfilesAsync(
			int? pageSize = null,
			string nextPageToken = null,
			string siteId = null,
			int? callingType = null,
			UserStatus? status = null,
			string department = null,
			string costCenter = null,
			string keyword = null,
			CancellationToken cancellationToken = default)
		{
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
				.AsObject<PhoneCallUserProfilesPaginationObject>();
		}

		#endregion
	}
}
