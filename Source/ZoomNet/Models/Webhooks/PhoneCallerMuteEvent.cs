using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered whenever the caller mutes themselves during a Zoom Phone call.
	/// </summary>
	public class PhoneCallerMuteEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the caller muted themselves.
		/// </summary>
		public DateTime MutedOn { get; set; }
	}
}
