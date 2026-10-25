using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered every time a host or meeting attendee changes their role during the meeting.</summary>
	public record MeetingParticipantRoleChangedEvent : MeetingParticipantEvent
	{
		/// <summary>Gets or sets the time when the participant changed the role.</summary>
		public DateTime ChangedOn { get; init; }

		/// <summary>Gets or sets the participant's new role.</summary>
		public ParticipantRole NewRole { get; init; }

		/// <summary>Gets or sets the participant's old role.</summary>
		public ParticipantRole OldRole { get; init; }
	}
}
