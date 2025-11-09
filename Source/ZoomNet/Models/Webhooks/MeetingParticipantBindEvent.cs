using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a phone user joins a meeting and binds to an attendee in the meeting.
	/// </summary>
	public class MeetingParticipantBindEvent : MeetingParticipantEvent
	{
		/// <summary>
		/// Gets or sets user id of participant the phone user binds to.
		/// </summary>
		/// <remarks>
		/// This value is assigned to a participant when they join a meeting, and is only valid for the duration of the meeting.
		/// </remarks>
		public string BindUserId { get; set; }

		/// <summary>
		/// Gets or sets uuid of participant the phone user binds to.
		/// </summary>
		public string BindParticipantUuid { get; set; }

		/// <summary>
		/// Gets or sets the time when the participant is bound.
		/// </summary>
		public DateTime BoundOn { get; set; }
	}
}
