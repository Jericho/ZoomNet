using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a host or attendee joins a webinar.</summary>
	public record WebinarParticipantJoinedEvent : WebinarParticipantEvent
	{
		/// <summary>Gets or sets the date and time at which the participant joined the webinar.</summary>
		public DateTime JoinedOn { get; init; }
	}
}
