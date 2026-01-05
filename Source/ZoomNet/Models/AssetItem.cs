using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents asset item model.
	/// </summary>
	public class AssetItem : AssetItemBase
	{
		/// <summary>
		/// Gets or sets the asset item's language code.
		/// </summary>
		[JsonPropertyName("asset_item_language")]
		public AssetLanguage Language { get; set; }

		/// <summary>
		/// Gets or sets the asset item's file URL.
		/// </summary>
		/// <remarks>
		/// This data only applies to <see cref="AssetType.Audio"/>, <see cref="AssetType.Image"/>, <see cref="AssetType.Video"/> and <see cref="AssetType.Slides"/>.
		/// </remarks>
		[JsonPropertyName("asset_item_file_url")]
		public string FileUrl { get; set; }

		/// <summary>
		/// Gets or sets the asset item's content.
		/// </summary>
		/// <remarks>
		/// This data only applies to <see cref="AssetType.Text"/> and <see cref="AssetType.SavedReply"/>.
		/// </remarks>
		[JsonPropertyName("asset_item_content")]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the asset item's text-to-speech voice.
		/// </summary>
		/// <remarks>
		/// This data only applies to <see cref="AssetType.Audio"/>.
		/// Not every language supports TTS. Each language has a different voice.
		/// </remarks>
		[JsonPropertyName("asset_item_voice")]
		public string Voice { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this item is the asset's default.
		/// Each asset can only have on default item.
		/// </summary>
		[JsonPropertyName("is_default")]
		public bool IsDefault { get; set; }
	}
}
