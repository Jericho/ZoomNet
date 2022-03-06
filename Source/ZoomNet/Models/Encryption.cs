using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the encryption.
	/// </summary>
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
