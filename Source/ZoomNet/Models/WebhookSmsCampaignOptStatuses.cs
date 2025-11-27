using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS campaign information that is sent in <see cref="Webhooks.PhoneSmsCampaignNumberOptInEvent"/> and
	/// <see cref="Webhooks.PhoneSmsCampaignNumberOptOutEvent"/> webhook events.
	/// </summary>
	public class WebhookSmsCampaignOptStatuses
	{
		/// <summary>
		/// Gets or sets the date and time when the event has occurred.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the list of phone number pairs that were opted in or out of SMS campaign.
		/// </summary>
		[JsonPropertyName("phone_number_campaign_opt_statuses")]
		public SmsCampaignNumbersOptStatus[] PhoneNumbers { get; set; }
	}
}
