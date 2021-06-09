using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of encryption.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EncryptionType
	{
		/// <summary>
		/// EnhancedEncryption.
		/// </summary>
		[EnumMember(Value = "enhanced-encryption")]
		EnhancedEncryption,

		/// <summary>
		/// EndToEndEncryption.
		/// </summary>
		[EnumMember(Value = "e2ee")]
		EndToEndEncryption
	}
}
