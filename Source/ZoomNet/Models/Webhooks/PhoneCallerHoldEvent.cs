using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered whenever the caller puts a Zoom Phone call on hold.
	/// </summary>
	public class PhoneCallerHoldEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the call hold began.
		/// </summary>
		public DateTime HoldStartedOn { get; set; }
	}
}
