using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>Represents an event related to a webinar.</summary>
	public abstract record WebinarInfoEvent : Event
	{
		/// <summary>Gets or sets the account id of the user who created the webinar.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets the information about the webinar.</summary>
		[JsonPropertyName("object")]
		public WebinarInfo Webinar { get; init; }
	}
}
