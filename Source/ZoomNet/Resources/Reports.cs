using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to view various metrics.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/reports">Zoom documentation</a> for more information.
	/// </remarks>
	public class Reports : IReports
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Reports" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Reports(IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Get a list of participants from past meetings with two or more participants. To see a list of participants for meetings with one participant use Dashboards.GetMeetingParticipantsAsync.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="includeFields">Participant fields to include. Allowed value: registrant_id.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="ReportParticipant">participants</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<ReportParticipant>> GetMeetingParticipantsAsync(string meetingId, string includeFields, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			if (pageSize < 1 || pageSize > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 300");
			}

			if (!string.IsNullOrEmpty(includeFields) && !includeFields.Equals("registrant_id".ToLowerInvariant()))
			{
				throw new ArgumentException("The only accepted value for includeFields is registrant_id");
			}
			else if (string.IsNullOrEmpty(includeFields))
			{
				includeFields = string.Empty;
			}

			return _client
				.GetAsync($"report/meetings/{meetingId}/participants")
				.WithArgument("include_fields", includeFields.ToLowerInvariant())
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<ReportParticipant>("participants");
		}
	}
}
