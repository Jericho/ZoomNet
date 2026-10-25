using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a webinar participant leaves the webinar.</summary>
	public record WebinarParticipantLeftEvent : WebinarParticipantEvent
	{
		/// <summary>Gets or sets the date and time at which the participant left the webinar.</summary>
		public DateTime LeftOn { get; init; }

		/// <summary>Gets or sets the reason why the participant left the webinar.</summary>
		public string LeaveReason { get; init; }
	}
}
