using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to specify whow can share their screen.
	/// </summary>
	public enum WhoCanShare
	{
		/// <summary>Only a host can share the screen.</summary>
		[EnumMember(Value = "host")]
		Host,

		/// <summary>Anyone in the meeting is allowed to start sharing their screen.</summary>
		[EnumMember(Value = "all")]
		Everyone
	}
}
