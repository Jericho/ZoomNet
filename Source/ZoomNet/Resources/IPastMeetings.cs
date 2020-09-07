using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage meetings that occured in the past.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/meetings/meetings">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IPastMeetings
	{
		/// <summary>
		/// Retrieve the details of a meeting that occured in the past.
		/// </summary>
		/// <param name="uuid">The meeting UUID.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Meeting" />.
		/// </returns>
		Task<Meeting> GetAsync(string uuid, CancellationToken cancellationToken = default);

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
		Task<PaginatedResponseWithToken<Participant>> GetParticipantsAsync(string uuid, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of ended meeting instance.
		/// </summary>
		/// <param name="meetingId">The meeting identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PastInstance" />.
		/// </returns>
		Task<PastInstance[]> GetInstancesAsync(long meetingId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of poll results for a meeting that occured in the past.
		/// </summary>
		/// <param name="meetingId">The meeting identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollResult" />.
		/// </returns>
		Task<PollResult[]> GetPollResultsAsync(long meetingId, CancellationToken cancellationToken = default);

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
		Task<MeetingFile[]> GetFilesAsync(long meetingId, CancellationToken cancellationToken = default);
	}
}
