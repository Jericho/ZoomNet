namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the incoming call log records are made available for the callee to view.
	/// </summary>
	public class PhoneCalleeCallLogCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets completed call logs information.
		/// </summary>
		public UserCallLog[] CallLogs { get; set; }
	}
}
