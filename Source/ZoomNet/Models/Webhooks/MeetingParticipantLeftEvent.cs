using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting participant leaves the meeting.
	/// </summary>
	public class MeetingParticipantLeftEvent : MeetingParticipantEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant left the meeting.
		/// </summary>
		public DateTime LeftOn { get; set; }

		/// <summary>
		/// Gets or sets the reason why the participant left the meeting.
		/// </summary>
		public string LeaveReason { get; set; }
	}
}
