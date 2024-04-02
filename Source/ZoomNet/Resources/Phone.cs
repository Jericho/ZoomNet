using Pathoschild.Http.Client;
using System;
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

		#endregion

		#region SMS endpoints

		/// <inheritdoc cref="IPhone.GetSmsSessionDetailsAsync" select="remarks" />
		public Task<PaginatedResponseWithToken<SmsHistory>> GetSmsSessionDetailsAsync(
			string sessionId, DateTime? from, DateTime? to, bool? orderAscending = true, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 100)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 100");
			}

			return _client
				.GetAsync($"phone/sms/sessions/{sessionId}")
				.WithArgument("sort", orderAscending.HasValue ? orderAscending.Value ? 1 : 2 : null)
				.WithArgument("from", from?.ToZoomFormat(dateOnly: false))
				.WithArgument("to", to?.ToZoomFormat(dateOnly: false))
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<SmsHistory>("sms_histories");
		}

		#endregion
	}
}
