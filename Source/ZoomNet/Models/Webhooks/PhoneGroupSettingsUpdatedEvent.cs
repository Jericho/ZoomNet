using System.Text.Json.Serialization;
using ZoomNet.Models.PhoneAccountSettings;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when group settings changed.</summary>
	public record PhoneGroupSettingsUpdatedEvent : Event
	{
		/// <summary>Gets or sets the account ID of the user that updated the group settings.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets the updated group settings.</summary>
		[JsonPropertyName("object")]
		public WebhookGroupSettings NewSettings { get; init; }

		/// <summary>Gets or sets the previous group settings (before update).</summary>
		[JsonPropertyName("old_object")]
		public WebhookGroupSettings OldSettings { get; init; }
	}
}
