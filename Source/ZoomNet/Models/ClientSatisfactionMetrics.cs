using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metrics for a client satisfaction item.</summary>
	public class ClientSatisfactionMetrics
	{
		/// <summary>Gets or sets the date of the report.</summary>
		[JsonPropertyName("date")]
		public DateTime Date { get; set; }

		/// <summary>Gets or sets the total number of "thumbs up" received for this meeting.</summary>
		[JsonPropertyName("good_count")]
		public int GoodCount { get; set; }

		/// <summary>Gets or sets the total number of attendees who didn't submit any response (neither thumbs up nor thumbs down).</summary>
		[JsonPropertyName("none_count")]
		public int NoneCount { get; set; }

		/// <summary>Gets or sets the total number of "thumbs down" received for this meeting.</summary>
		[JsonPropertyName("not_good_count")]
		public int NotGoodCount { get; set; }

		/// <summary>Gets or sets the satisfaction percentage. <br/> The satisfaction percentage is calculated as `(good_count + none_count)` / `total_count`.</summary>
		[JsonPropertyName("satisfaction_percent")]
		public long SatisfactionPercent { get; set; }
	}
}
