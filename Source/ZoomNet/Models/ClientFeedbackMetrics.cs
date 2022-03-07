using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics for a feedback item.
	/// </summary>
	public class ClientFeedbackMetrics
	{
		/// <summary>
		/// Gets or sets the feedback id.
		/// </summary>
		/// <value>The feedback id.</value>
		[JsonPropertyName("feedback_id")]
		public string FeedbackId { get; set; }

		/// <summary>
		/// Gets or sets the feedback name.
		/// </summary>
		/// <value>The feedback name.</value>
		[JsonPropertyName("feedback_name")]
		public string FeebackName { get; set; }

		/// <summary>
		/// Gets or sets the participant count.
		/// </summary>
		/// <value>The number of participants that upvoted the feedback.</value>
		[JsonPropertyName("participants_count")]
		public int ParticipantsCount { get; set; }
	}
}
