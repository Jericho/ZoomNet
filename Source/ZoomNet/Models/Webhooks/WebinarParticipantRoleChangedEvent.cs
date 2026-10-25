using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered every time a host or webinar attendee changes their role during a webinar.</summary>
	public record WebinarParticipantRoleChangedEvent : WebinarParticipantEvent
	{
		/// <summary>Gets or sets the time when the participant changed the role.</summary>
		public DateTime ChangedOn { get; init; }

		/// <summary>Gets or sets the participant's new role.</summary>
		public ParticipantRole NewRole { get; init; }

		/// <summary>Gets or sets the participant's old role.</summary>
		public ParticipantRole OldRole { get; init; }
	}
}
