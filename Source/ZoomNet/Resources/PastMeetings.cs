using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage meetings that occured in the past.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.IMeetings" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/meetings/meetings">Zoom documentation</a> for more information.
	/// </remarks>
	public class PastMeetings : IPastMeetings
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="PastMeetings" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal PastMeetings(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve the details of a meeting that occured in the past.
		/// </summary>
		/// <param name="uuid">The meeting UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Meeting" />.
		/// </returns>
		public Task<Meeting> GetAsync(string uuid, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{uuid}")
				.WithCancellationToken(cancellationToken)
				.AsObject<Meeting>();
		}

		/// <summary>
		/// List participants of a meeting that occured in the past.
		/// </summary>
		/// <param name="uuid">The meeting UUID.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Participant" />.
		/// </returns>
		public Task<PaginatedResponseWithToken<Participant>> GetParticipantsAsync(string uuid, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"past_meetings/{uuid}/participants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Participant>("participants");
		}

		/// <summary>
		/// Get a list of ended meeting instance.
		/// </summary>
		/// <param name="meetingId">The meeting identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PastInstance" />.
		/// </returns>
		public Task<PastInstance[]> GetInstancesAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{meetingId}/instances")
				.WithCancellationToken(cancellationToken)
				.AsObject<PastInstance[]>("meetings");
		}

		/// <summary>
		/// Get a list of poll results for a meeting that occured in the past.
		/// </summary>
		/// <param name="meetingId">The meeting identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollResult" />.
		/// </returns>
		public Task<PollResult[]> GetPollResultsAsync(long meetingId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_meetings/{meetingId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollResult[]>("questions");
		}

		/// <summary>
		/// Get a list of files sent via in-meeting chat during a meeting.
		/// </summary>
		/// <remarks>
		/// The in-meeting files are deleted after 24 hours of the meeting completion time.
		/// </remarks>
		/// <param name="meetingId">The meeting identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="MeetingFile" />.
		/// </returns>
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
