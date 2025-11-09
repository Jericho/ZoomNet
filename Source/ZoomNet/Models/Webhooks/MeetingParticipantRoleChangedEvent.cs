using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a host or meeting attendee changes their role during the meeting.
	/// </summary>
	public class MeetingParticipantRoleChangedEvent : MeetingParticipantEvent
	{
		/// <summary>
		/// Gets or sets the time when the participant changed the role.
		/// </summary>
		public DateTime ChangedOn { get; set; }

		/// <summary>
		/// Gets or sets the participant's new role.
		/// </summary>
		public ParticipantRole NewRole { get; set; }

		/// <summary>
		/// Gets or sets the participant's old role.
		/// </summary>
		public ParticipantRole OldRole { get; set; }
	}
}
