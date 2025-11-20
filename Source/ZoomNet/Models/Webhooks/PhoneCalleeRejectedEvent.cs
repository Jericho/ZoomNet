using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the callee declines an incoming Zoom Phone call.
	/// </summary>
	public class PhoneCalleeRejectedEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the ringing started.
		/// </summary>
		public DateTime RingingStartedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the call was declined by the callee.
		/// </summary>
		public DateTime EndedOn { get; set; }

		/// <summary>
		/// Gets or sets the reason the call was rejected.
		/// </summary>
		public string HandupResult { get; set; }
	}
}
