using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the quality of a connection during a meeting.
	/// </summary>
	public enum QualityType
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>
		/// Good.
		/// </summary>
		[EnumMember(Value = "good")]
		Good,

		/// <summary>
		/// Fair.
		/// </summary>
		[EnumMember(Value = "fair")]
		Fair,

		/// <summary>
		/// Poor.
		/// </summary>
		[EnumMember(Value = "poor")]
		Poor,

		/// <summary>
		/// Bad.
		/// </summary>
		[EnumMember(Value = "bad")]
		Bad
	}
}
