using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the recordings has been initiated in a phone call.
	/// </summary>
	public class PhoneRecordingStartedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets information about started recording.
		/// </summary>
		[JsonPropertyName("object")]
		public PhoneCallRecording Recording { get; set; }
	}
}
