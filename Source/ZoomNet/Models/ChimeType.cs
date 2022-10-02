using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of chime.
	/// </summary>
	public enum ChimeType
	{
		/// <summary>
		/// Play sound when host joins or leaves.
		/// </summary>
		[EnumMember(Value = "host")]
		HostOnly,

		/// <summary>
		/// Play sound when any participant joins or leaves.
		/// </summary>
		[EnumMember(Value = "all")]
		All,

		/// <summary>
		/// No join or leave sound.
		/// </summary>
		[EnumMember(Value = "none")]
		None
	}
}
