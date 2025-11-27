using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when an account user receives an SMS message with text of an opt-in keyword, such as START.
	/// </summary>
	public class PhoneSmsCampaignNumberOptInEvent : PhoneSmsEvent
	{
		/// <summary>
		/// Gets or sets information about phone numbers that sent and received opt-in keyword.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookSmsCampaignOptStatuses SmsCampaignNumbers { get; set; }
	}
}
