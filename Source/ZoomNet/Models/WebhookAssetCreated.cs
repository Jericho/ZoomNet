using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Payload model for <see cref="Webhooks.ContactCenterAssetCreatedEvent"/>.</summary>
	public record WebhookAssetCreated
	{
		/// <summary>Gets or sets the asset's description.</summary>
		[JsonPropertyName("asset_description")]
		public string Description { get; init; }

		/// <summary>Gets or sets the asset's id.</summary>
		[JsonPropertyName("asset_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the asset's items.</summary>
		[JsonPropertyName("asset_items")]
		public AssetItem[] Items { get; init; }

		/// <summary>Gets or sets the asset's language code.</summary>
		[JsonPropertyName("asset_language_code")]
		public Language LanguageCode { get; init; }

		/// <summary>Gets or sets the asset's name.</summary>
		[JsonPropertyName("asset_name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the asset's type.</summary>
		[JsonPropertyName("asset_type")]
		public AssetType Type { get; init; }

		/// <summary>Gets or sets the asset category's id.</summary>
		[JsonPropertyName("category_id")]
		public string CategoryId { get; init; }

		/// <summary>Gets or sets the asset category's name.</summary>
		[JsonPropertyName("category_name")]
		public string CategoryName { get; init; }

		/// <summary>Gets or sets the date and time when the asset was created.</summary>
		[JsonPropertyName("date_time_ms")]
		public DateTime CreatedOn { get; init; }

		/// <summary>Gets or sets the id of the user that last modified this data.</summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; init; }
	}
}
