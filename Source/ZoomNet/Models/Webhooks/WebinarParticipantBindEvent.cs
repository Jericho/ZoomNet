using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a phone user joins a webinar and binds to an attendee in the webinar.
	/// </summary>
	public class WebinarParticipantBindEvent : WebinarParticipantEvent
	{
		/// <summary>
		/// Gets or sets user id of participant the phone user binds to.
		/// </summary>
		/// <remarks>
		/// This value is assigned to a participant when they join a webinar, and is only valid for the duration of the webinar.
		/// </remarks>
		public string BindUserId { get; set; }

		/// <summary>
		/// Gets or sets uuid of participant the phone user binds to.
		/// </summary>
		public string BindParticipantUuid { get; set; }

		/// <summary>
		/// Gets or sets the time when the participant joined the webinar.
		/// </summary>
		public DateTime JoinTime { get; set; }
	}
}
