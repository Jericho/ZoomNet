using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to the SMS (campaign, sending and receiving and so on).
	/// </summary>
	public abstract class PhoneSmsEvent : Event
	{
		/// <summary>
		/// Gets or sets owner account id.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }
	}
}
