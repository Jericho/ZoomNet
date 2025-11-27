using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the recording of a phone call fails.
	/// </summary>
	public class PhoneRecordingFailedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets information about failed recording.
		/// </summary>
		[JsonPropertyName("object")]
		public PhoneCallRecording Recording { get; set; }
	}
}
