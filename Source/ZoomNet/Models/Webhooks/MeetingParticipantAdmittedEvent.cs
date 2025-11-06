using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting host admits the participant from a waiting room to the meeting.
	/// </summary>
	public class MeetingParticipantAdmittedEvent : MeetingParticipantEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant has been admitted to the meeting.
		/// </summary>
		public DateTime AdmittedOn { get; set; }
	}
}
