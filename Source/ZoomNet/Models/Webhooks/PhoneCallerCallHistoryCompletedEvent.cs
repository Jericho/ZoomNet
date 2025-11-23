namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the outgoing call history records are made available for the caller to view.
	/// </summary>
	public class PhoneCallerCallHistoryCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets completed call elements information.
		/// </summary>
		public CallElement[] CallLogs { get; set; }
	}
}
