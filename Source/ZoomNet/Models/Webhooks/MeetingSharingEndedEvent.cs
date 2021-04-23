using Newtonsoft.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an attendee or the host stops sharing their screen.
	/// </summary>
	public class MeetingSharingEndedEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the information about the participant and the screenshare.
		/// </summary>
		[JsonProperty(PropertyName = "participant")]
		public ScreenshareSharingDetails ScreenshareDetails { get; set; }
	}
}
