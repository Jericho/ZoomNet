namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a transcription of a call recording has completed.
	/// </summary>
	public class PhoneRecordingTranscriptCompletedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets completed phone call recordings.
		/// </summary>
		public PhoneCallRecording[] Recordings { get; set; }
	}
}
