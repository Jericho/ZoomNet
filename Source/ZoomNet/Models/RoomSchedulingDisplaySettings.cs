using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room scheduling display settings.
	/// </summary>
	public class RoomSchedulingDisplaySettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow a user to perform an instant room reservation on the device.
		/// </summary>
		[JsonPropertyName("instant_room_reservation")]
		public bool? AllowInstantRoomReservation { get; set; }

		/// <summary>
		/// Gets or sets the settings that allows Zoom Room Scheduling Display to reserve other Zoom Rooms.
		/// </summary>
		[JsonPropertyName("allow_to_reserve_other_rooms")]
		public RoomLocationReserveOtherRoomsSettings ReserveOtherRooms { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to use the same background image as the Zoom Room.
		/// </summary>
		/// <remarks>
		/// Some Zoom Rooms Scheduling displays use a different display aspect ratio than standard 16:9; the background image will be scaled to fit the Scheduling Display's display, with any excess cropped.
		/// </remarks>
		[JsonPropertyName("set_scheduling_display_background_image_to_zoom_room_background")]
		public bool? UseZoomRoomBackgroundImage { get; set; }

		/// <summary>
		/// Gets or sets the Zoom Room scheduling display UI theme.
		/// </summary>
		[JsonPropertyName("home_screen_theme")]
		public string HomeScreenTheme { get; set; }

		/// <summary>
		/// Gets or sets Url where users are redirected when the QR code image is scanned.
		/// </summary>
		/// <remarks>
		/// If the Zoom Rooms Scheduling Display home screen theme is "compact", a space is reserved for a QR code to be displayed.
		/// You may optionally provide a URL which Zoom will be used to automatically generate the corresponding QR code image.
		/// If you provide a URL that exceeds 100 characters in total length, Zoom will automatically use the Zoom URL shortening service to generate a shortened URL that will redirect to your actual URL; the URL is shortened to increase QR code scan reliability.
		/// This setting has no effect if the home screen theme is "standard".
		/// </remarks>
		[JsonPropertyName("home_screen_qr_code_url")]
		public string QRCodeUrl { get; set; }

		/// <summary>
		/// Gets or sets thew additional text to appear adjacent to the space reserved for a QR code.
		/// </summary>
		/// <remarks>
		/// If the Zoom Rooms Scheduling Display home screen theme is "compact", a space is reserved for additional text to appear adjacent to the space reserved for a QR code.
		/// This setting has no effect if the home screen theme is "standard".
		/// </remarks>
		[JsonPropertyName("home_screen_qr_code_supporting_text")]
		public string QRCodeSupportingText { get; set; }
	}
}
