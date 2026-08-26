using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metrics for a feedback item.</summary>
	public class ClientFeedbackMetrics
	{
		/// <summary>Gets or sets the feedback id.</summary>
		[JsonPropertyName("feedback_id")]
		public string FeedbackId { get; set; }

		/// <summary>Gets or sets the feedback name.</summary>
		[JsonPropertyName("feedback_name")]
		public string FeebackName { get; set; }

		/// <summary>Gets or sets the participant count.</summary>
		[JsonPropertyName("participants_count")]
		public int ParticipantsCount { get; set; }
	}
}
