namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a recording has been deleted.</summary>
	public record PhoneRecordingDeletedEvent : RecordingEvent
	{
		/// <summary>Gets or sets deleted recordings information.</summary>
		public PhoneCallRecordingBasicInfo[] Recordings { get; init; }
	}
}
