namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a recording has been deleted permanently.</summary>
	public record PhoneRecordingPermanentlyDeletedEvent : RecordingEvent
	{
		/// <summary>Gets or sets deleted recordings information.</summary>
		public PhoneCallRecordingBasicInfo[] Recordings { get; init; }
	}
}
