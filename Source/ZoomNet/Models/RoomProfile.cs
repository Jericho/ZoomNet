using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room Profile.
	/// </summary>
	public class RoomProfile
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the activation code.
		/// </summary>
		[JsonProperty("activation_code")]
		public string ActivationCode { get; set; }

		/// <summary>
		/// Gets or sets the support email.
		/// </summary>
		[JsonProperty("support_email")]
		public string SupportEmail { get; set; }

		/// <summary>
		/// Gets or sets the support phone number.
		/// </summary>
		[JsonProperty("support_phone")]
		public string SupportPhone { get; set; }

		/// <summary>
		/// Gets or sets the room passcode.
		/// </summary>
		[JsonProperty("room_passcode")]
		public string RoomPasscode { get; set; }

		/// <summary>
		/// Gets or sets a value to determine if the application requires a code to exit and switch to other apps.
		/// </summary>
		[JsonProperty("required_code_to_ext")]
		public bool RequiredCodeToExit { get; set; }

		/// <summary>
		/// Gets or sets a value to determine if the room is hidden in the contact list.
		/// </summary>
		[JsonProperty("hide_room_in_contacts")]
		public bool HideRoomInContacts { get; set; }
	}
}
