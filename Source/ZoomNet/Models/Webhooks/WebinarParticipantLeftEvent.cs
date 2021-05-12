using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a webinar participant leaves the webinar.
	/// </summary>
	public class WebinarParticipantLeftEvent : WebinarEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the participant left the webinar.
		/// </summary>
		public DateTime LeftOn { get; set; }

		/// <summary>
		/// Gets or sets the participant.
		/// </summary>
		public WebhookParticipant Participant { get; set; }
	}
}
