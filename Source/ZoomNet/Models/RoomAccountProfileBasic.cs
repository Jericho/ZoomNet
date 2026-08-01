using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom Room account profile basic information.
	/// </summary>
	public class RoomAccountProfileBasic
	{
		/// <summary>
		/// Gets or sets the code required to exit the Zoom Room.
		/// </summary>
		[JsonPropertyName("required_code_to_exit")]
		public string RequiredCodeToExit { get; set; }

		/// <summary>
		/// Gets or sets the passcode for the Zoom Room.
		/// </summary>
		[JsonPropertyName("room_passcode")]
		public string RoomPasscode { get; set; }

		/// <summary>
		/// Gets or sets the support email for the Zoom Room.
		/// </summary>
		[JsonPropertyName("support_email")]
		public string SupportEmail { get; set; }

		/// <summary>
		/// Gets or sets the support phone number for the Zoom Room.
		/// </summary>
		[JsonPropertyName("support_phone")]
		public string SupportPhone { get; set; }
	}
}
