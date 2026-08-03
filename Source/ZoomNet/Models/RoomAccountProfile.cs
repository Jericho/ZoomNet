using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom Room account profile.
	/// </summary>
	public class RoomAccountProfile
	{
		/// <summary>
		/// Gets or sets the basic profile information.
		/// </summary>
		[JsonPropertyName("basic")]
		public RoomAccountProfileBasic BasicProfile { get; set; }

		/// <summary>
		/// Gets or sets the setup profile information.
		/// </summary>
		[JsonPropertyName("setup")]
		public RoomAccountProfileSetup SetupProfile { get; set; }
	}
}
