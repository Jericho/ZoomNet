using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a call log is deleted (sent to the trash).</summary>
	[Obsolete("This event is deprecated and will be removed in a future release. phone.call_element_deleted — triggers when a call element record is deleted, which supersedes the legacy call log deletion payload with a forward-compatible data model.")]
	public class PhoneCallLogDeletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>Gets or sets information about deleted call logs.</summary>
		public CallLogBasicInfo[] CallLogs { get; set; }
	}
}
