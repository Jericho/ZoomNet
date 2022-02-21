using System.Text.Json.Serialization;
using ZoomNet.Resources;

namespace ZoomNet.Models
{
	/// <summary>
	/// Pagination Object for meeting metrics returned as deep nested properties in some <see cref="IDashboards"/> endpoints.
	/// </summary>
	public class DashboardMeetingMetricsPaginationObject
	{
		/// <summary>
		/// Gets or sets the start date for the report.
		/// </summary>
		/// <value> Start date for this report in 'yyyy-MM-dd' format.</value>
		public string From { get; set; }

		/// <summary>
		/// Gets or sets the end date for the report.
		/// </summary>
		/// <value> End date for this report in 'yyyy-MM-dd' format.</value>
		public string To { get; set; }

		/// <summary>
		/// Gets or sets the number of items returned on this page.
		/// </summary>
		/// <value>The number of items returned on this page.</value>
		[JsonPropertyName("page_count")]
		public int PageCount { get; set; }

		/// <summary>
		/// Gets or sets the number of records returned within a single API call.
		/// </summary>
		/// <value>The number of records returned within a single API call.</value>
		[JsonPropertyName("page_size")]
		public int PageSize { get; set; }

		/// <summary>
		/// Gets or sets the number of all records available across pages.
		/// </summary>
		/// <value>The number of all records available across pages.</value>
		[JsonPropertyName("total_records")]
		public int? TotalRecords { get; set; }

		/// <summary>
		/// Gets or sets the token to retrieve the next page.
		/// </summary>
		/// <value>The page token.</value>
		/// <remarks>This token expires after 15 minutes.</remarks>
		[JsonPropertyName("next_page_token")]
		public string NextPageToken { get; set; }

		/// <summary>
		/// Gets or sets the metrics for the meetings.
		/// </summary>
		/// <value>The metrics for the meetings.</value>
		[JsonPropertyName("meetings")]
		public DashboardMeetingMetrics[] Records { get; set; }
	}
}
