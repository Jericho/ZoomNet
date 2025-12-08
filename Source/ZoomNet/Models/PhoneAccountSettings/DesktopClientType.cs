using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Desktop client types.
	/// </summary>
	public enum DesktopClientType
	{
		/// <summary>MacOS client.</summary>
		[EnumMember(Value = "mac_os")]
		MacOs,

		/// <summary>Windows client.</summary>
		[EnumMember(Value = "windows")]
		Windows,

		/// <summary>Virtual Desktop Infrastructure (VDI) client.</summary>
		[EnumMember(Value = "vdi_client")]
		VirtualDesktop,

		/// <summary>Linux client.</summary>
		[EnumMember(Value = "linux")]
		Linux,
	}
}
