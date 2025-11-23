using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered whenever the callee unmutes themselves during a Zoom Phone call.
	/// </summary>
	public class PhoneCalleeUnmuteEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the callee unmuted themselves.
		/// </summary>
		public DateTime UnmutedOn { get; set; }
	}
}
