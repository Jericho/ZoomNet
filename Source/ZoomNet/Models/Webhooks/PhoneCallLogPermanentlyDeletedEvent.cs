namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a call log is permanently deleted from the trash.
	/// </summary>
	public class PhoneCallLogPermanentlyDeletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets information about deleted call logs.
		/// </summary>
		public CallLogBasicInfo[] CallLogs { get; set; }
	}
}
