using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The edited version of a meeting AI summary.
	/// </summary>
	public class MeetingAiSummaryEdited
	{
		/// <summary>
		/// Gets or sets the edited summary overview.
		/// </summary>
		[JsonPropertyName("summary_overview")]
		public string SummaryOverview { get; set; }

		/// <summary>
		/// Gets or sets the edited summary details as a string.
		/// </summary>
		[JsonPropertyName("summary_details")]
		public string SummaryDetails { get; set; }

		/// <summary>
		/// Gets or sets the edited next steps.
		/// </summary>
		[JsonPropertyName("next_steps")]
		public string[] NextSteps { get; set; }
	}
}
