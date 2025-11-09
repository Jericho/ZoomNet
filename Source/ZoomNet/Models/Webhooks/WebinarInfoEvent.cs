using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to a webinar.
	/// </summary>
	public abstract class WebinarInfoEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who created the webinar.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the information about the webinar.
		/// </summary>
		[JsonPropertyName("object")]
		public WebinarInfo Webinar { get; set; }
	}
}
