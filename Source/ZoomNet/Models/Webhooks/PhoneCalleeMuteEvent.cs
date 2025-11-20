using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered whenever the callee mutes themselves during a Zoom Phone call.
	/// </summary>
	public class PhoneCalleeMuteEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the callee muted themselves.
		/// </summary>
		public DateTime MutedOn { get; set; }
	}
}
