using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the caller hears a ringing tone on the phone device.
	/// </summary>
	public class PhoneCallerRingingEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the ringing started.
		/// </summary>
		public DateTime RingingStartedOn { get; set; }
	}
}
