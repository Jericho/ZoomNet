using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of encryption.
	/// </summary>
	public enum EncryptionType
	{
		/// <summary>Enhanced encryption.</summary>
		/// <remarks>Encryption data is stored in the cloud.</remarks>
		[EnumMember(Value = "enhanced_encryption ")]
		Enhanced,

		/// <summary>End-to-end encryption.</summary>
		/// <remarks>
		/// The encryption key is stored on the local device and cannot be obtained by anyone else.
		/// Enabling end-to-end encryption also disables certain features, such as cloud recording, live streaming, and allowing participants to join before the host.
		/// </remarks>
		[EnumMember(Value = "e2ee")]
		EndToEnd
	}
}
