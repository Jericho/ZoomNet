using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the updated asset.
	/// </summary>
	public class WebhookAssetUpdates
	{
		/// <summary>
		/// Gets or sets the asset's name.
		/// </summary>
		[JsonPropertyName("asset_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the asset's description.
		/// </summary>
		[JsonPropertyName("asset_description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the asset category's id.
		/// </summary>
		[JsonPropertyName("category_id")]
		public string CategoryId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this asset is restored.
		/// </summary>
		[JsonPropertyName("restore")]
		public bool? IsRestored { get; set; }

		/// <summary>
		/// Gets or sets the added asset's items.
		/// </summary>
		[JsonPropertyName("asset_item_added")]
		public AssetItem[] AddedItems { get; set; }

		/// <summary>
		/// Gets or sets the deleted asset's items.
		/// </summary>
		[JsonPropertyName("asset_item_deleted")]
		public AssetItemBase[] DeletedItems { get; set; }

		/// <summary>
		/// Gets or sets the updated asset's items.
		/// </summary>
		[JsonPropertyName("asset_item_updated")]
		public AssetItem[] UpdatedItems { get; set; }
	}
}
