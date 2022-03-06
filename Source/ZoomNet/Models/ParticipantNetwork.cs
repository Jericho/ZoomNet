using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of network a participant used to join a meeting.
	/// </summary>
	public enum ParticipantNetwork
	{
		/// <summary>
		/// An unknown network.
		/// </summary>
		[EnumMember(Value = "Others")]
		Other,

		/// <summary>
		/// Wired.
		/// </summary>
		[EnumMember(Value = "Wired")]
		Wired,

		/// <summary>
		/// Wifi.
		/// </summary>
		[EnumMember(Value = "Wifi")]
		Wifi,

		/// <summary>
		/// Point-to-point.
		/// </summary>
		[EnumMember(Value = "PPP")]
		PPP,

		/// <summary>
		/// 3G, 4G and 5G cellular.
		/// </summary>
		[EnumMember(Value = "Cellular")]
		Cellular
	}
}
