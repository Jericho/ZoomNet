using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Elements you want to dispay in the top banner.
	/// </summary>
	public class SignageBannerSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to display the room name.
		/// </summary>
		[JsonPropertyName("banner_room_name")]
		public bool DisplayRoomName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display the sharing key.
		/// </summary>
		[JsonPropertyName("banner_sharing_key")]
		public bool DisplaySharingKey { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display the time.
		/// </summary>
		[JsonPropertyName("banner_time")]
		public bool DisplayTime { get; set; }
	}
}
