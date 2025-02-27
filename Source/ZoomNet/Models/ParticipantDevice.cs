using System.Runtime.Serialization;
using ZoomNet.Utilities;

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
		[MultipleValuesEnumMember(DefaultValue = "Windows", OtherValues = new[] { "WIN", "win 10", "win 11", "Windows 10", "Windows 11" })]
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
		Android,

		/// <summary>
		/// The participant joined via the web.
		/// </summary>
		[EnumMember(Value = "Web")]
		Web,

		/// <summary>
		/// The participant joined via a Zoom Room.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms")]
		ZoomRoom
	}
}
