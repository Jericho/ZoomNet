using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage webinars that occured in the past.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/webinars/">Zoom documentation</a> for more information.
	/// </remarks>
	public interface IPastWebinars
	{
		/// <summary>
		/// List absentees of a webinar that occured in the past.
		/// </summary>
		/// <param name="uuid">The webinar UUID.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Registrant" />.
		/// </returns>
		Task<PaginatedResponseWithToken<Registrant>> GetAbsenteesAsync(string uuid, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// List all the participants who attended a webinar hosted in the past.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Participant" />.
		/// </returns>
		Task<PaginatedResponseWithToken<Participant>> GetParticipantsAsync(long webinarId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of ended webinar instance.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PastInstance" />.
		/// </returns>
		Task<PastInstance[]> GetInstancesAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of poll results for a webinar that occured in the past.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollResult" />.
		/// </returns>
		Task<PollResult[]> GetPollResultsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of Q&amp;A results for a webinar that occured in the past.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollResult" />.
		/// </returns>
		Task<PollResult[]> GetQuestionsAndAnswersResultsAsync(long webinarId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a list of files sent via in-webinar chat during a webinar.
		/// </summary>
		/// <remarks>
		/// The in-webinar files are deleted after 24 hours of the webinar completion time.
		/// </remarks>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="MeetingFile" />.
		/// </returns>
		Task<MeetingFile[]> GetFilesAsync(long webinarId, CancellationToken cancellationToken = default);
	}
}
