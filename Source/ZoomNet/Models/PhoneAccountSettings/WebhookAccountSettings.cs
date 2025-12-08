using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Account settings as provided in <see cref="Webhooks.PhoneAccountSettingsUpdatedEvent"/>.
	/// </summary>
	public class WebhookAccountSettings
	{
		/// <summary>
		/// Gets or sets the updated account's id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the account settings.
		/// </summary>
		[JsonPropertyName("settings")]
		public AccountSettings Settings { get; set; }
	}
}
