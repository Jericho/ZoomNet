using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the callee parks a Zoom Phone call.
	/// </summary>
	public class PhoneCalleeParkedEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the call park began.
		/// </summary>
		public DateTime ParkedOn { get; set; }

		/// <summary>
		/// Gets or sets the code to connect with caller.
		/// </summary>
		public string ParkCode { get; set; }

		/// <summary>
		/// Gets or sets the reason for parking failure.
		/// </summary>
		public string ParkFailureReason { get; set; }
	}
}
