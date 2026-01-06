using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Payload model for <see cref="Webhooks.ContactCenterAssetCreatedEvent"/>.
	/// </summary>
	public class WebhookAssetCreated
	{
		/// <summary>
		/// Gets or sets the asset's id.
		/// </summary>
		[JsonPropertyName("asset_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the asset's name.
		/// </summary>
		[JsonPropertyName("asset_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the asset's type.
		/// </summary>
		[JsonPropertyName("asset_type")]
		public AssetType Type { get; set; }

		/// <summary>
		/// Gets or sets the asset's language code.
		/// </summary>
		[JsonPropertyName("asset_language_code")]
		public Language LanguageCode { get; set; }

		/// <summary>
		/// Gets or sets the asset's description.
		/// </summary>
		[JsonPropertyName("asset_description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the asset was created.
		/// </summary>
		[JsonPropertyName("date_time_ms")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the id of the user that last modified this data.
		/// </summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

		/// <summary>
		/// Gets or sets the asset category's id.
		/// </summary>
		[JsonPropertyName("category_id")]
		public string CategoryId { get; set; }

		/// <summary>
		/// Gets or sets the asset category's name.
		/// </summary>
		[JsonPropertyName("category_name")]
		public string CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the asset's items.
		/// </summary>
		[JsonPropertyName("asset_items")]
		public AssetItem[] Items { get; set; }
	}
}
