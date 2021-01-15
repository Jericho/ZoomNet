using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the encryption.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Encryption
	{
		/// <summary>
		/// Auto.
		/// </summary>
		[EnumMember(Value = "auto")]
		Auto,

		/// <summary>
		/// Yes.
		/// </summary>
		[EnumMember(Value = "yes")]
		Yes,

		/// <summary>
		/// No.
		/// </summary>
		[EnumMember(Value = "no")]
		No
	}
}
