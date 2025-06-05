using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location setup profile.
	/// </summary>
	public class RoomLocationSetupProfile
	{
		/// <summary>
		/// Gets or sets a value indicating whether to apply the same background image to all displays of the Zoom Room.
		/// If the value of the this field is true, the background_image_info object will only contain and only accept changes to the background image information of zoom_rooms_display1.
		/// </summary>
		[JsonPropertyName("apply_background_image_to_all_displays")]
		public bool? ApplyBackgroundImageToAllDisplays { get; set; }

		/// <summary>
		/// Gets or sets the background image information for each display.
		/// If the value of the <see cref="ApplyBackgroundImageToAllDisplays"/> is true, this object will only accept changes to the background image information of zoom_rooms_display1.
		/// </summary>
		[JsonPropertyName("background_image_info")]
		public RoomLocationBackgroundImageInfo[] BackgroundImageInfo { get; set; }
	}
}
