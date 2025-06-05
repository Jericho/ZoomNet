using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Room security settings.
	/// </summary>
	public class RoomSecuritySettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the meeting password is encrypted and included in the invitation link.
		/// </summary>
		[JsonPropertyName("encryption_type")]
		public EncryptionType? EncryptionType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether end-to-end encryption is enabled.
		/// </summary>
		[JsonPropertyName("end_to_end_encrypted_meetings")]
		public bool? EndToEndEncryptionEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether all meetings must be secured with at least one security options.
		/// </summary>
		/// <remarks>
		/// The available security options are:
		///   - a passcode
		///   - Waiting Room
		///   - "Only authenticated users can join meetings"
		/// If no security option is enabled, Zoom will secure all meetings with Waiting Room.
		/// </remarks>
		[JsonPropertyName("auto_security")]
		public bool? MustBeSecured { get; set; }
	}
}
