using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a conference call has started.
	/// </summary>
	public class PhoneConferenceStartedEvent : Event
	{
		/// <summary>
		/// Gets or sets the user's account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets conference call information.
		/// </summary>
		[JsonPropertyName("object")]
		public ConferenceCall ConferenceCall { get; set; }
	}
}
