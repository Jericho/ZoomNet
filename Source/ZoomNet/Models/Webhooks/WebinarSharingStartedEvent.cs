using Newtonsoft.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an app user or account user starts sharing their screen during a webinar.
	/// </summary>
	public class WebinarSharingStartedEvent : WebinarEvent
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
