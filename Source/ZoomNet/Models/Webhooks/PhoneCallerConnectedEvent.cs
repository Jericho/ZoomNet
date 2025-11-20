using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a connection starts between the caller and callee on Zoom Phone.
	/// </summary>
	public class PhoneCallerConnectedEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the ringing started.
		/// </summary>
		public DateTime RingingStartedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the connection was established.
		/// </summary>
		public DateTime ConnectedOn { get; set; }
	}
}
