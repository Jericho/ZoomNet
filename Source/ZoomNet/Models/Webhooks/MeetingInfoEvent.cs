using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a meeting.
	/// </summary>
	public abstract class MeetingInfoEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who created the meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about the meeting.
		/// </summary>
		[JsonPropertyName("object")]
		public MeetingInfo Meeting { get; set; }
	}
}
