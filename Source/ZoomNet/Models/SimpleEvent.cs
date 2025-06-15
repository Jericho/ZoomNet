using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A sinlge session event.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.Event" />
	public class SimpleEvent : Event
	{
		/// <summary>Gets or sets the type of meeting.</summary>
		[JsonPropertyName("meeting_type")]
		public EventMeetingType MeetingType { get; set; }
	}
}
