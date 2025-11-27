namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a recording of a Zoom phone call completes for access member.
	/// </summary>
	public class PhoneRecordingCompletedForAccessMemberEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets completed phone call recordings.
		/// </summary>
		public PhoneCallRecording[] Recordings { get; set; }
	}
}
