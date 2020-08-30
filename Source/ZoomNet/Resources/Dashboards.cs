using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;
using ZoomNet.Utilities;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to view various metrics.
	/// </summary>
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/zoom-api/dashboards">Zoom documentation</a> for more information.
	/// </remarks>
	public class Dashboards : IDashboards
	{
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dashboards" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal Dashboards(IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve data on total live or past meetings that occurred during a specified period of time.
		/// Only data from within the last 6 months will be returned.
		/// </summary>
		/// <param name="from">
		/// Date to start searching from. Should be within a month of "to" as only a months worth of data is returned at a time.
		/// </param>
		/// <param name="to">Date to end search.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="pageSize">The number of records returned within a single API call.</param>
		/// <param name="pageToken">
		/// The next page token is used to paginate through large result sets.
		/// A next page token will be returned whenever the set of available results exceeds the current page size.
		/// The expiration period for this token is 15 minutes.
		/// </param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="DashboardMeeting">meetings</see>.
		/// </returns>
		public Task<PaginatedResponseWithToken<DashboardMeeting>> GetAllAsync(DateTime from, DateTime to, MeetingListType type = MeetingListType.Live, int pageSize = 30, string pageToken = null, CancellationToken cancellationToken = default)
		{
			if (pageSize < 1 || pageSize > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 300");
			}

			return _client
				.GetAsync($"metrics/meetings")
				.WithArgument("type", JToken.Parse(JsonConvert.SerializeObject(type)).ToString())
				.WithArgument("from", from.ToString("yyyy-MM-dd"))
				.WithArgument("to", to.ToString("yyyy-MM-dd"))
				.WithArgument("page_size", pageSize)
				.WithArgument("next_page_token", pageToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<DashboardMeeting>("meetings");
		}

		/// <summary>
		/// Retrieve the details of a meeting.
		/// </summary>
		/// <param name="meetingId">The meeting ID or meeting UUID. If given the meeting ID it will take the last meeting instance.</param>
		/// <param name="type">The type of meetings. Allowed values: Past, PastOne, Live.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="DashboardMeeting" />.
		/// </returns>
		public Task<DashboardMeeting> GetMeetingAsync(string meetingId, MeetingListType type = MeetingListType.Live, CancellationToken cancellationToken = default)
		{
			return _client
				.GetAsync($"metrics/meetings/{meetingId}")
				.WithArgument("type", JToken.Parse(JsonConvert.SerializeObject(type)).ToString())
				.WithCancellationToken(cancellationToken)
				.AsObject<DashboardMeeting>();
		}
}
