using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a webinar.
	/// </summary>
	public abstract class WebinarEvent : Event
	{
		/// <summary>
		/// Gets or sets the unique identifier of the account in which the event occurred.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the webinar object.
		/// </summary>
		[JsonPropertyName("object")]
		public Webinar Webinar { get; set; }
	}
}
