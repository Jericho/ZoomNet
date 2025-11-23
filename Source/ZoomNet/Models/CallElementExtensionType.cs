using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Extension type used in call element.
	/// </summary>
	public enum CallElementExtensionType
	{
		/// <summary>User.</summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>Call queue.</summary>
		[EnumMember(Value = "call_queue")]
		CallQueue,

		/// <summary>Auto receptionist.</summary>
		[EnumMember(Value = "auto_receptionist")]
		AutoReceptionist,

		/// <summary>Common area.</summary>
		[EnumMember(Value = "common_area")]
		CommonArea,

		/// <summary>Zoom room.</summary>
		[EnumMember(Value = "zoom_room")]
		ZoomRoom,

		/// <summary>Cisco room.</summary>
		[EnumMember(Value = "cisco_room")]
		CiscoRoom,

		/// <summary>Shared line group.</summary>
		[EnumMember(Value = "shared_line_group")]
		SharedLineGroup,

		/// <summary>Group call pickup.</summary>
		[EnumMember(Value = "group_call_pickup")]
		GroupCallPickup,

		/// <summary>External contact.</summary>
		[EnumMember(Value = "external_contact")]
		ExternalContact,
	}
}
