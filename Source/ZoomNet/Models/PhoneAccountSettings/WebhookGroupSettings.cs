using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>Group settings as provided in <see cref="Webhooks.PhoneGroupSettingsUpdatedEvent"/>.</summary>
	public record WebhookGroupSettings
	{
		/// <summary>Gets or sets the group id.</summary>
		[JsonPropertyName("group_id")]
		public string GroupId { get; init; }

		/// <summary>Gets or sets the group settings.</summary>
		[JsonPropertyName("settings")]
		public GroupSettings Settings { get; init; }
	}
}
