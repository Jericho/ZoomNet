namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a recording has been deleted permanently.
	/// </summary>
	public class PhoneRecordingPermanentlyDeletedEvent : RecordingEvent
	{
		/// <summary>
		/// Gets or sets deleted recordings information.
		/// </summary>
		public PhoneCallRecordingBasicInfo[] Recordings { get; set; }
	}
}
