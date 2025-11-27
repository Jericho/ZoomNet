using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS message information (as provided in webhook events).
	/// </summary>
	public class WebhookSmsMessage : SmsMessageBase
	{
		/// <summary>
		/// Gets or sets the SMS session id.
		/// </summary>
		[JsonPropertyName("session_id")]
		public string SessionId { get; set; }

		/// <summary>
		/// Gets or sets the reason for SMS sending failure.
		/// </summary>
		/// <remarks>
		/// Empty field means the SMS sending was successful.
		/// </remarks>
		[JsonPropertyName("failure_reason")]
		public string FailureReason { get; set; }

		/// <summary>
		/// Gets or sets the owner of the SMS.
		/// </summary>
		[JsonPropertyName("owner")]
		public WebhookSmsOwner Owner { get; set; }

		/// <summary>
		/// Gets or sets the SMS sender.
		/// </summary>
		[JsonPropertyName("sender")]
		public WebhookSmsParticipant Sender { get; set; }

		/// <summary>
		/// Gets or sets the SMS receivers.
		/// </summary>
		[JsonPropertyName("to_members")]
		public WebhookSmsParticipant[] Recipients { get; set; }

		/// <summary>
		/// Gets or sets the list of opt statuses for each number pair.
		/// </summary>
		[JsonPropertyName("phone_number_campaign_opt_statuses")]
		public SmsCampaignNumbersOptStatus[] PhoneNumbersOptStatuses { get; set; }
	}
}
