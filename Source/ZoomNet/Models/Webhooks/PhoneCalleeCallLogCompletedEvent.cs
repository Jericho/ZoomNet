using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when the incoming call log records are made available for the callee to view.</summary>
	[Obsolete("This event is deprecated and will be removed in a future release. phone.callee_call_element_completed — delivers the incoming call element record for the callee, which supersedes the legacy call log payload with a richer, forward-compatible data model.")]
	public class PhoneCalleeCallLogCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>Gets or sets completed call logs information.</summary>
		public UserCallLog[] CallLogs { get; set; }
	}
}
