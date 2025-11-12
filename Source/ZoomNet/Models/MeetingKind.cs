using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Supported meeting kinds - internal and external.
	/// </summary>
	public enum MeetingKind
	{
		/// <summary>
		/// Meeting is internal.
		/// </summary>
		[EnumMember(Value = "internal")]
		Internal,

		/// <summary>
		/// Meeting is external (includes participant from outside organization).
		/// </summary>
		[EnumMember(Value = "external")]
		External,
	}
}
