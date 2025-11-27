using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the owner pauses the recording during a phone call.
	/// </summary>
	public class PhoneRecordingPausedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets information about paused recording.
		/// </summary>
		[JsonPropertyName("object")]
		public PhoneCallRecording Recording { get; set; }
	}
}
