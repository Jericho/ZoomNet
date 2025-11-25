using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the owner resumes the recording during a phone call.
	/// </summary>
	public class PhoneRecordingResumedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets information about resumed recording.
		/// </summary>
		[JsonPropertyName("object")]
		public PhoneCallRecording Recording { get; set; }
	}
}
