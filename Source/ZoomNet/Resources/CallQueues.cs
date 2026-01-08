using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class CallQueues : ICallQueues
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CallQueues" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CallQueues(IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<CallQueue> GetAsync(string callQueueId, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"phone/call_queues/{callQueueId}")
				.WithCancellationToken(cancellationToken)
				.AsObject<CallQueue>();
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<CallQueue>> GetAllAsync(string department = null, string cost_center = null, string site_id = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage, max: 100);

			return _client
				.GetAsync($"phone/call_queues")
				.WithArgument("department", department)
				.WithArgument("cost_center", cost_center)
				.WithArgument("site_id", site_id)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<CallQueue>("call_queues");
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<CallQueueMember>> GetMembersAsync(string callQueueId, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			Utils.ValidateRecordPerPage(recordsPerPage);
			return _client
				.GetAsync($"phone/call_queues/{callQueueId}/members")
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<CallQueueMember>("call_queue_members");
		}
	}
}
