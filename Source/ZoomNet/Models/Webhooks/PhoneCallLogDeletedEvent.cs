namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a call log is deleted (sent to the trash).</summary>
	public record PhoneCallLogDeletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>Gets or sets information about deleted call logs.</summary>
		public CallLogBasicInfo[] CallLogs { get; init; }
	}
}
