using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the Contact Center queue channel.
	/// </summary>
	public enum ContactCenterQueueChannel
	{
		/// <summary>Voice.</summary>
		[EnumMember(Value = "voice")]
		Voice,

		/// <summary>Video.</summary>
		[EnumMember(Value = "video")]
		Video,

		/// <summary>Mesaging.</summary>
		[EnumMember(Value = "messaging")]
		Messaging,

		/// <summary>Email.</summary>
		[EnumMember(Value = "email")]
		Email,
	}
}
