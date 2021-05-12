using Newtonsoft.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an attendee or the host stops sharing their screen during a meeting.
	/// </summary>
	public class MeetingSharingEndedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the information about the participant.
		/// </summary>
		[JsonProperty(PropertyName = "participant")]
		public WebhookParticipant Participant { get; set; }

		/// <summary>
		/// Gets or sets the information about the screenshare.
		/// </summary>
		[JsonProperty(PropertyName = "sharing_details")]
		public ScreenshareDetails ScreenshareDetails { get; set; }
	}
}
