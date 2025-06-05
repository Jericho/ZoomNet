using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class PastMeetings : IPastMeetings
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="PastMeetings" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal PastMeetings(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PastMeeting> GetAsync(string meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{meetingId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<PastMeeting>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Participant>> GetParticipantsAsync(string meetingId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"past_meetings/{meetingId}/participants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Participant>("participants");
		}

		/// <inheritdoc/>
		public Task<PastInstance[]> GetInstancesAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{meetingId}/instances")
				.WithCancellationToken(cancellationToken)
				.AsObject<PastInstance[]>("meetings");
		}

		/// <inheritdoc/>
		public Task<PollResult[]> GetPollResultsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{meetingId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollResult[]>("questions");
		}

		/// <inheritdoc/>
		[Obsolete("This method has been deprecated and is no longer supported due to GCM encryption updates for security purposes")]
		public Task<MeetingFile[]> GetFilesAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{meetingId}/files")
				.WithCancellationToken(cancellationToken)
				.AsObject<MeetingFile[]>("in_meeting_files");
		}
	}
}
