namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when the incoming call history records are made available for the callee to view.</summary>
	public record PhoneCalleeCallHistoryCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>Gets or sets completed call elements information.</summary>
		public CallElement[] CallLogs { get; set; }
	}
}
