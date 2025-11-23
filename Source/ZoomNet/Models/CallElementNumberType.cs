using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Number type used in call element.
	/// </summary>
	public enum CallElementNumberType
	{
		/// <summary>Zoom PSTN.</summary>
		[EnumMember(Value = "zoom_pstn")]
		ZoomPstn,

		/// <summary>Zoom toll-free number.</summary>
		[EnumMember(Value = "zoom_toll_free_number")]
		ZoomTollFreeNumber,

		/// <summary>External PSTN.</summary>
		[EnumMember(Value = "external_pstn")]
		ExternalPstn,

		/// <summary>External contact.</summary>
		[EnumMember(Value = "external_contact")]
		ExternalContact,

		/// <summary>Bring your own carrier.</summary>
		[EnumMember(Value = "byoc")]
		Byoc,

		/// <summary>Bring your own PBX.</summary>
		[EnumMember(Value = "byop")]
		Byop,

		/// <summary>Third party contact center.</summary>
		[EnumMember(Value = "3rd_party_contact_center")]
		ThirdPartyContactCenter,

		/// <summary>Zoom service number.</summary>
		[EnumMember(Value = "zoom_service_number")]
		ZoomServiceNumber,

		/// <summary>External service number.</summary>
		[EnumMember(Value = "external_service_number")]
		ExternalServiceNumber,

		/// <summary>Zoom contact center.</summary>
		[EnumMember(Value = "zoom_contact_center")]
		ZoomContactCenter,

		/// <summary>Meeting phone number.</summary>
		[EnumMember(Value = "meeting_phone_number")]
		MeetingPhoneNumber,

		/// <summary>Meeting id.</summary>
		[EnumMember(Value = "meeting_id")]
		MeetingId,

		/// <summary>Anonymous number.</summary>
		[EnumMember(Value = "anonymous_number")]
		AnonymousNumber,
	}
}
