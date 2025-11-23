using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered whenever the caller unmutes themselves during a Zoom Phone call.
	/// </summary>
	public class PhoneCallerUnmuteEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the caller unmuted themselves.
		/// </summary>
		public DateTime UnmutedOn { get; set; }
	}
}
