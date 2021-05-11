using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a webinar participant joins the webinar.
	/// </summary>
	public class WebinarParticipantJoinedEvent : WebinarEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant joined the meeting.
		/// </summary>
		public DateTime JoinedOn { get; set; }

		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
