using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents meeting information as received in AI companion webhook events.
	/// </summary>
	public class WebhookAiCompanionMeetingInfo
	{
		/// <summary>
		/// Gets or sets the meeting id.
		/// </summary>
		[JsonPropertyName("meeting_number")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the unique meeting id.
		/// </summary>
		[JsonPropertyName("meeting_uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the id of the user set as the host of the meeting.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }
	}
}
