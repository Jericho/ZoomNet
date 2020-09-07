using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics for a client satisfaction item.
	/// </summary>
	public class ClientSatisfactionMetrics
	{
		/// <summary>
		/// Gets or sets the date of the report.
		/// </summary>
		/// <value>The date of the report.</value>
		[JsonProperty(PropertyName = "date")]
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the satisfaction percentage. <br/>
		/// The satisfaction percentage is calculated as `(good_count + none_count)` / `total_count`.
		/// </summary>
		/// <value>The satisfaction percentage.</value>
		[JsonProperty(PropertyName = "satisfaction_percent")]
		public long SatisfactionPercent { get; set; }

		/// <summary>
		/// Gets or sets the total number of "thumbs up" received for this meeting.
		/// </summary>
		/// <value>The total number of "thumbs up" received for this meeting.</value>
		[JsonProperty(PropertyName = "good_count")]
		public int GoodCount { get; set; }

		/// <summary>
		/// Gets or sets the total number of "thumbs down" received for this meeting.
		/// </summary>
		/// <value>The total number of "thumbs down" received for this meeting.</value>
		[JsonProperty(PropertyName = "not_good_count")]
		public int NotGoodCount { get; set; }

		/// <summary>
		/// Gets or sets the total number of attendees who didn't submit any response (neither thumbs up nor thumbs down).
		/// </summary>
		/// <value>The total number of attendees who didn't submit any response (neither thumbs up nor thumbs down).</value>
		[JsonProperty(PropertyName = "none_count")]
		public int NoneCount { get; set; }
	}
}
