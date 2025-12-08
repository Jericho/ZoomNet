using System.Text.Json.Serialization;
using ZoomNet.Models.PhoneAccountSettings;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when account settings are updated.
	/// </summary>
	public class PhoneAccountSettingsUpdatedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account ID of the user that updated the account settings.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the updated account settings.
		/// </summary>
		[JsonPropertyName("object")]
		public WebhookAccountSettings NewSettings { get; set; }

		/// <summary>
		/// Gets or sets the previous account settings (before update).
		/// </summary>
		[JsonPropertyName("old_object")]
		public WebhookAccountSettings OldSettings { get; set; }
	}
}
