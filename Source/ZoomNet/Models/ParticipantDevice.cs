using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of device a participant used to join a meeting.
	/// </summary>
	public enum ParticipantDevice
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		[EnumMember(Value = "Unknown")]
		Unknown,

		/// <summary>
		/// The participant joined via PSTN.
		/// </summary>
		[EnumMember(Value = "Phone")]
		Phone,

		/// <summary>
		/// The participant joined via an H.323 or SIP device.
		/// </summary>
		[EnumMember(Value = "H.323/SIP")]
		Sip,

		/// <summary>
		/// The participant joined via VoIP using a Windows device.
		/// </summary>
		[EnumMember(Value = "Windows")]
		Windows,

		/// <summary>
		/// The participant joined via VoIP using a Mac device.
		/// </summary>
		[EnumMember(Value = "Mac")]
		Mac,

		/// <summary>
		/// The participant joined via VoIP using a iOS device.
		/// </summary>
		[EnumMember(Value = "iOs")]
		IOS,

		/// <summary>
		/// The participant joined via VoIP using an Android device.
		/// </summary>
		[EnumMember(Value = "Android")]
		Android
	}
}
