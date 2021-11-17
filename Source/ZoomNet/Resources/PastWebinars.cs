using Pathoschild.Http.Client;
using System;
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
	public class PastWebinars : IPastWebinars
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="PastWebinars" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal PastWebinars(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

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
		public Task<PaginatedResponseWithToken<Registrant>> GetAbsenteesAsync(string uuid, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"past_webinars/{uuid}/absentees")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

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
		public Task<PaginatedResponseWithToken<Participant>> GetParticipantsAsync(long webinarId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"past_webinars/{webinarId}/participants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Participant>("participants");
		}

		/// <summary>
		/// Get a list of ended webinar instance.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PastInstance" />.
		/// </returns>
		public Task<PastInstance[]> GetInstancesAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/instances")
				.WithCancellationToken(cancellationToken)
				.AsObject<PastInstance[]>("webinars");
		}

		/// <summary>
		/// Get a list of poll results for a webinar that occured in the past.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollResult" />.
		/// </returns>
		public Task<PollResult[]> GetPollResultsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollResult[]>("questions");
		}

		/// <summary>
		/// Get a list of Q&amp;A results for a webinar that occured in the past.
		/// </summary>
		/// <param name="webinarId">The webinar identifier.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="PollResult" />.
		/// </returns>
		public Task<PollResult[]> GetQuestionsAndAnswersResultsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/qa")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollResult[]>("questions");
		}

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
		public Task<MeetingFile[]> GetFilesAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/files")
				.WithCancellationToken(cancellationToken)
				.AsObject<MeetingFile[]>("in_meeting_files");
		}
	}
}
