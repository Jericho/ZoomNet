using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to the phone call flow between caller and callee.
	/// </summary>
	public abstract class PhoneCallFlowEvent : Event
	{
		/// <summary>
		/// Gets or sets account id of the caller or callee depending on the webhook event.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets information about the call.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookPhoneCallInfo CallInfo { get; set; }
	}
}
