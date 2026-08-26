using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Zoom Room account profile setup information.</summary>
	public class RoomAccountProfileSetup
	{
		/// <summary>Gets or sets a value indicating whether to apply the background image to all displays.</summary>
		[JsonPropertyName("apply_background_image_to_all_displays")]
		public bool ApplyBackgroundImageToAllDisplays { get; set; }

		/// <summary>Gets or sets the background image information for the Zoom Room.</summary>
		[JsonPropertyName("background_image_info")]
		public RoomLocationBackgroundImageInfo[] BackgroundImages { get; set; }
	}
}
