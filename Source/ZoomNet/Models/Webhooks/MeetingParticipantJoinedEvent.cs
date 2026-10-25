using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when an attendee joins a meeting.</summary>
	public record MeetingParticipantJoinedEvent : MeetingParticipantEvent
	{
		/// <summary>Gets or sets the date and time at which the participant joined the meeting.</summary>
		public DateTime JoinedOn { get; init; }
	}
}
