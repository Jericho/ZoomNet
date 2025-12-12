using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Forwarding extension type.
	/// </summary>
	public enum ForwardingExtensionType
	{
		/// <summary>Zoom user.</summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>Zoom room.</summary>
		[EnumMember(Value = "zoomRoom")]
		ZoomRoom,

		/// <summary>Common area phone.</summary>
		[EnumMember(Value = "commonArea")]
		CommonArea,

		/// <summary>Cisco/Polycom room.</summary>
		[EnumMember(Value = "ciscoRoom/polycomRoom")]
		CiscoPolycomRoom,

		/// <summary>Zoom auto receptionist.</summary>
		[EnumMember(Value = "autoReceptionist")]
		AutoReceptionist,

		/// <summary>Shared line group.</summary>
		[EnumMember(Value = "sharedLineGroup")]
		SharedLineGroup,

		/// <summary>Zoom call queue.</summary>
		[EnumMember(Value = "callQueue")]
		CallQueue,
	}
}
