using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents basic asset item properties.
	/// </summary>
	public class AssetItemBase
	{
		/// <summary>
		/// Gets or sets the asset's item id.
		/// </summary>
		[JsonPropertyName("asset_item_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the asset's item name.
		/// </summary>
		[JsonPropertyName("asset_item_name")]
		public string Name { get; set; }
	}
}
