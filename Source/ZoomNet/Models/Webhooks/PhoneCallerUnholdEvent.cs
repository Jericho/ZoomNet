using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered whenever the caller unholds the Zoom Phone call.</summary>
	public record PhoneCallerUnholdEvent : PhoneCallFlowEvent
	{
		/// <summary>Gets or sets the date and time when the call hold ended.</summary>
		public DateTime HoldEndedOn { get; init; }
	}
}
