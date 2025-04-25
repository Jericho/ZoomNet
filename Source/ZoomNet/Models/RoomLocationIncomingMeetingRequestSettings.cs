using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location incoming meeting requests settings.
	/// </summary>
	public class RoomLocationIncomingMeetingRequestSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Room will automatically accept incoming meeting requests (calls), such as being invited to a meeting in progress.
		/// </summary>
		[JsonPropertyName("automatically_accept_incoming_meeting_request")]
		public bool? AutomaticallyAcceptIncomingMeetingRequests { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the Zoom Room will automatically unmute after it automatically accepts the incoming meeting request and joins the meeting.
		/// </summary>
		[JsonPropertyName("automatically_unmute")]
		public bool? AutomaticallyUnmute { get; set; }
	}
}
