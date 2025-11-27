using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an account user receives an SMS message with text of an opt-out keyword, such as STOP.
	/// </summary>
	public class PhoneSmsCampaignNumberOptOutEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets information about phone numbers that sent and received opt-out keyword.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsCampaignOptStatuses SmsCampaignNumbers { get; set; }
	}
}
