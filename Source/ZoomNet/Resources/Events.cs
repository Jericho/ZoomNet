using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <inheritdoc/>
	public class Events : IEvents
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Events" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Events(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <inheritdoc/>
		public Task<PaginatedResponseWithToken<Event>> GetAllAsync(int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"zoom_events/events")
				//.WithArgument("role_type", role.ToEnumString())
				//.WithArgument("events_status_typeroletype", status.ToEnumString())
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<Event>("events");
		}
	}
}
