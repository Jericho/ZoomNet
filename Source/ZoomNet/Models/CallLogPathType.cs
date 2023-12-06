using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the extension type.
	/// </summary>
	public enum CallLogPathType
	{
		/// <summary>
		/// voiceMail.
		/// </summary>
		[EnumMember(Value = "voiceMail")]
		VoiceMail,

		/// <summary>
		/// message.
		/// </summary>
		[EnumMember(Value = "message")]
		Message,

		/// <summary>
		/// forward.
		/// </summary>
		[EnumMember(Value = "forward")]
		Forward,

		/// <summary>
		/// extension.
		/// </summary>
		[EnumMember(Value = "extension")]
		Extension,

		/// <summary>
		/// callQueue.
		/// </summary>
		[EnumMember(Value = "callQueue")]
		CallQueue,

		/// <summary>
		/// ivrMenu.
		/// </summary>
		[EnumMember(Value = "ivrMenu")]
		IvrMenu,

		/// <summary>
		/// companyDirectory.
		/// </summary>
		[EnumMember(Value = "companyDirectory")]
		CompanyDirectory,

		/// <summary>
		/// autoReceptionist.
		/// </summary>
		[EnumMember(Value = "autoReceptionist")]
		AutoReceptionist,

		/// <summary>
		/// contactCenter.
		/// </summary>
		[EnumMember(Value = "contactCenter")]
		ContactCenter,

		/// <summary>
		/// disconnected.
		/// </summary>
		[EnumMember(Value = "disconnected")]
		Disconnected,

		/// <summary>
		/// commonAreaPhone.
		/// </summary>
		[EnumMember(Value = "commonAreaPhone")]
		CommonAreaPhone,

		/// <summary>
		/// pstn.
		/// </summary>
		[EnumMember(Value = "pstn")]
		Pstn,

		/// <summary>
		/// transfer.
		/// </summary>
		[EnumMember(Value = "transfer")]
		Transfer,

		/// <summary>
		/// sharedLines.
		/// </summary>
		[EnumMember(Value = "sharedLines")]
		SharedLines,

		/// <summary>
		/// sharedLineGroup.
		/// </summary>
		[EnumMember(Value = "sharedLineGroup")]
		SharedLineGroup,

		/// <summary>
		/// tollFreeBilling.
		/// </summary>
		[EnumMember(Value = "tollFreeBilling")]
		TollFreeBilling,

		/// <summary>
		/// meetingService.
		/// </summary>
		[EnumMember(Value = "meetingService")]
		MeetingService,

		/// <summary>
		/// parkPickup.
		/// </summary>
		[EnumMember(Value = "parkPickup")]
		ParkPickup,

		/// <summary>
		/// parkTimeout.
		/// </summary>
		[EnumMember(Value = "parkTimeout")]
		ParkTimeout,

		/// <summary>
		/// monitor.
		/// </summary>
		[EnumMember(Value = "monitor")]
		Monitor,

		/// <summary>
		/// takeover.
		/// </summary>
		[EnumMember(Value = "takeover")]
		Takeover,

		/// <summary>
		/// sipGroup.
		/// </summary>
		[EnumMember(Value = "sipGroup")]
		SipGroup
	}
}
