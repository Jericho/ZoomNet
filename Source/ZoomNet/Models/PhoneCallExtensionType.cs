using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone number extension type.
	/// </summary>
	public enum PhoneCallExtensionType
	{
		/// <summary>
		/// Zoom user.
		/// </summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>
		/// Zoom call queue.
		/// </summary>
		[EnumMember(Value = "callQueue")]
		CallQueue,

		/// <summary>
		/// Zoom auto receptionist.
		/// </summary>
		[EnumMember(Value = "autoReceptionist")]
		AutoReceptionist,

		/// <summary>
		/// Common area.
		/// </summary>
		[EnumMember(Value = "commonArea")]
		CommonArea,

		/// <summary>
		/// Common area phone.
		/// </summary>
		[EnumMember(Value = "commonAreaPhone")]
		CommonAreaPhone,

		/// <summary>
		/// Shared line group.
		/// </summary>
		[EnumMember(Value = "sharedLineGroup")]
		SharedLineGroup,

		/// <summary>
		/// Shared lines.
		/// </summary>
		[EnumMember(Value = "sharedLines")]
		SharedLines,

		/// <summary>
		/// Zoom room.
		/// </summary>
		[EnumMember(Value = "zoomRoom")]
		ZoomRoom,

		/// <summary>
		/// Cisco/Polycom room.
		/// </summary>
		[EnumMember(Value = "ciscoRoom/PolycomRoom")]
		CiscoPolycomRoom,

		/// <summary>
		/// Zoom contact center.
		/// </summary>
		[EnumMember(Value = "contactCenter")]
		ContactCenter,

		/// <summary>
		/// PSTN (public switched telephone network).
		/// </summary>
		[EnumMember(Value = "pstn")]
		Pstn,

		/// <summary>
		/// Five9 (cloud contact center provider).
		/// </summary>
		[EnumMember(Value = "five9")]
		Five9,

		/// <summary>
		/// Twilio (cloud communications company).
		/// </summary>
		[EnumMember(Value = "twilio")]
		Twilio,
	}
}
