using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a poll.
	/// </summary>
	public enum PollStatus
	{
		/// <summary>
		/// Poll has not started.
		/// </summary>
		[EnumMember(Value = "notstart")]
		NotStarted,

		/// <summary>
		/// Poll has started.
		/// </summary>
		[EnumMember(Value = "started")]
		Started,

		/// <summary>
		/// Poll has ended.
		/// </summary>
		[EnumMember(Value = "ended")]
		Ended,

		/// <summary>
		/// Sharing poll results.
		/// </summary>
		[EnumMember(Value = "sharing")]
		SharingResults
	}
}
