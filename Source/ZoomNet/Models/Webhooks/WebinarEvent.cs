using Newtonsoft.Json;
using ZoomNet.Utilities;

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
		[JsonProperty(PropertyName = "account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the webinar object.
		/// </summary>
		[JsonProperty(PropertyName = "object")]
		[JsonConverter(typeof(WebinarConverter))]
		public Webinar Webinar { get; set; }
	}
}
