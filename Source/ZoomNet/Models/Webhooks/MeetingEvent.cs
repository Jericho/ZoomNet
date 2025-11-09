using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting.
	/// </summary>
	public abstract class MeetingEvent : Event
	{
		/// <summary>
		/// Gets or sets the unique identifier of the account in which the event occurred.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the meeting object.
		/// </summary>
		[JsonPropertyName("object")]
		public Meeting Meeting { get; set; }
	}
}
