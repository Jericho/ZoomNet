using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room setup profile.
	/// </summary>
	public class RoomSetupProfile
	{
		/// <summary>
		/// Gets or sets the set up information for checking in and out of a Zoom Room.
		/// </summary>
		[JsonPropertyName("checkin_and_checkout")]
		public RoomCheckinCheckoutConfig CheckinAndCheckout { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the "under construction" mode is enabled.
		/// Rooms with under construction mode enabled are hidden on the dashboard by default and all issue notifications are disabled.
		/// You can change your dashboard filters to show rooms under construction if you prefer.
		/// </summary>
		[JsonPropertyName("under_construction")]
		public bool UnderConstruction { get; set; }

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

		/// <summary>
		/// Gets or sets a value indicating whether the Personal Zoom Room will consume a Zoom Rooms license and have access to "Pro" features.
		/// Note: this field will only be present for Personal Zoom Rooms.
		/// </summary>
		[JsonPropertyName("pro_device")]
		public bool? IsProDevice { get; set; }
	}
}
