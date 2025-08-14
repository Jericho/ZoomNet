using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room signage settings.
	/// </summary>
	public class RoomSignageSettings
	{
		/// <summary>
		/// Gets or sets the elements that you want to display in the top banner.
		/// </summary>
		[JsonPropertyName("banner")]
		public SignageBannerSettings Banner { get; set; }

		/// <summary>
		/// Gets or sets the number of minutes before or after the scheduled meeting time you would like the content to display.
		/// </summary>
		[JsonPropertyName("display_period")]
		public SignageBannerDisplayPeriod DisplayPeriod { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether digital signage is on or off.
		/// </summary>
		[JsonPropertyName("enable_digital_signage")]
		public bool? EnableDigitalSignage { get; set; }

		/// <summary>
		/// Gets or sets the layout.
		/// </summary>
		[JsonPropertyName("layout")]
		public SignageLayout? Layout { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether sounds of all contents is muted.
		/// </summary>
		[JsonPropertyName("mute")]
		public bool? IsMuted { get; set; }

		/// <summary>
		/// Gets or sets the content lists.
		/// </summary>
		[JsonPropertyName("play_list")]
		public SignageContentList[] ContentLists { get; set; }
	}
}
