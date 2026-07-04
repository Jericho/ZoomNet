using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A section of a meeting AI summary.
	/// </summary>
	public class MeetingAiSummarySection
	{
		/// <summary>
		/// Gets or sets the label of the summary section.
		/// </summary>
		[JsonPropertyName("label")]
		public string Label { get; set; }

		/// <summary>
		/// Gets or sets the content of the summary section.
		/// </summary>
		[JsonPropertyName("summary")]
		public string Summary { get; set; }
	}
}
