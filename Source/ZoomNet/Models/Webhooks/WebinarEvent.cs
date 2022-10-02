using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a webinar.
	/// </summary>
	public class WebinarEvent : Event
	{
		/// <summary>
		/// Gets or sets the unique identifier of the account in wich the event occured.
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
