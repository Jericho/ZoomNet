using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Group settings as provided in <see cref="Webhooks.PhoneGroupSettingsUpdatedEvent"/>.
	/// </summary>
	public class WebhookGroupSettings
	{
		/// <summary>
		/// Gets or sets the group id.
		/// </summary>
		[JsonPropertyName("group_id")]
		public string GroupId { get; set; }

		/// <summary>
		/// Gets or sets the group settings.
		/// </summary>
		[JsonPropertyName("settings")]
		public GroupSettings Settings { get; set; }
	}
}
