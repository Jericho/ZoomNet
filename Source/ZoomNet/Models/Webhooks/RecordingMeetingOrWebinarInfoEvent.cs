using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting or webinar recording.
	/// </summary>
	public abstract class RecordingMeetingOrWebinarInfoEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets information about recorded meeting or webinar.
		/// </summary>
		[JsonPropertyName("object")]
		public RecordedMeetingOrWebinarInfo SessionInfo { get; set; }
	}
}
