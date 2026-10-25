using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered whenever the caller escalates a Zoom Phone call to a Zoom meeting.</summary>
	public record PhoneCallerMeetingInvitingEvent : PhoneCallFlowEvent
	{
		/// <summary>Gets or sets the date and time when the call was escalated to a meeting.</summary>
		public DateTime EscalatedAt { get; init; }

		/// <summary>Gets or sets the meeting id.</summary>
		public string MeetingId { get; init; }
	}
}
