using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class PastWebinars : IPastWebinars
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="PastWebinars" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal PastWebinars(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Registrant>> GetAbsenteesAsync(string uuid, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"past_webinars/{uuid}/absentees")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Registrant>("registrants");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Participant>> GetParticipantsAsync(long webinarId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);

			return _client
				.GetAsync($"past_webinars/{webinarId}/participants")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Participant>("participants");
		}

		/// <inheritdoc/>
		public Task<PastInstance[]> GetInstancesAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/instances")
				.WithCancellationToken(cancellationToken)
				.AsObject<PastInstance[]>("webinars");
		}

		/// <inheritdoc/>
		public Task<PollResult[]> GetPollResultsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/polls")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollResult[]>("questions");
		}

		/// <inheritdoc/>
		public Task<PollResult[]> GetQuestionsAndAnswersResultsAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/qa")
				.WithCancellationToken(cancellationToken)
				.AsObject<PollResult[]>("questions");
		}

		/// <inheritdoc/>
		public Task<MeetingFile[]> GetFilesAsync(long webinarId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"past_webinars/{webinarId}/files")
				.WithCancellationToken(cancellationToken)
				.AsObject<MeetingFile[]>("in_meeting_files");
		}
	}
}
