using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom Room account profile.
	/// </summary>
	public class ZoomRoomAccountProfile
	{
		/// <summary>
		/// Gets or sets the basic profile information.
		/// </summary>
		[JsonPropertyName("basic")]
		public ZoomRoomAccountProfileBasic BasicProfile { get; set; }

		/// <summary>
		/// Gets or sets the setup profile information.
		/// </summary>
		[JsonPropertyName("setup")]
		public ZoomRoomAccountProfileSetup SetupProfile { get; set; }
	}
}
