using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the caller ends a Zoom Phone call.
	/// </summary>
	public class PhoneCallerEndedEvent : PhoneCallFlowEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the ringing started.
		/// </summary>
		public DateTime RingingStartedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the callee answered the call.
		/// </summary>
		public DateTime? AnsweredOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the call was ended by the caller.
		/// </summary>
		public DateTime EndedOn { get; set; }

		/// <summary>
		/// Gets or sets the reason the call was ended.
		/// </summary>
		public string HandupResult { get; set; }
	}
}
