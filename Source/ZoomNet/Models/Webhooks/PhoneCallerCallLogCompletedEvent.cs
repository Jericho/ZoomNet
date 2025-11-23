namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the outgoing call log records are made available for the caller to view.
	/// </summary>
	public class PhoneCallerCallLogCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets completed call logs information.
		/// </summary>
		public UserCallLog[] CallLogs { get; set; }
	}
}
