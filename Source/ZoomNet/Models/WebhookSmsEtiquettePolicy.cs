using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// SMS etiquette policy information that is sent in <see cref="Webhooks.PhoneSmsEtiquetteBlockEvent"/> and
	/// <see cref="Webhooks.PhoneSmsEtiquetteWarnEvent"/> webhook events.
	/// </summary>
	public class WebhookSmsEtiquettePolicy
	{
		/// <summary>
		/// Gets or sets the date and time when the event has occurred.
		/// </summary>
		[JsonPropertyName("date_time")]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the email of a Zoom phone user that violated SMS etiquette policy.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the message that violated SMS etiquette policy.
		/// </summary>
		[JsonPropertyName("message")]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the name of the SMS etiquette policy that was violated.
		/// </summary>
		[JsonPropertyName("policy_name")]
		public string PolicyName { get; set; }
	}
}
