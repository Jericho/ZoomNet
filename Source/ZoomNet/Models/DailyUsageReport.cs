using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Daily Usage Report.</summary>
	public record DailyUsageReport
	{
		/// <summary>Gets or sets the daily usage summaries.</summary>
		[JsonPropertyName("dates")]
		public DailyUsageSummary[] DailyUsageSummaries { get; init; }

		/// <summary>Gets or sets the month.</summary>
		[JsonPropertyName("month")]
		public int Month { get; init; }

		/// <summary>Gets or sets the year.</summary>
		[JsonPropertyName("year")]
		public int Year { get; init; }
	}
}
