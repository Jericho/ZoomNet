using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the extension type.
	/// </summary>
	public enum PhoneNumberExtensionType
	{
		/// <summary>
		/// user.
		/// </summary>
		[EnumMember(Value = "user")]
		User,

		/// <summary>
		/// callQueue.
		/// </summary>
		[EnumMember(Value = "callQueue")]
		CallQueue,

		/// <summary>
		/// autoReceptionist.
		/// </summary>
		[EnumMember(Value = "autoReceptionist")]
		AutoReceptionist,

		/// <summary>
		/// commonArea.
		/// </summary>
		[EnumMember(Value = "commonArea")]
		CommonArea,

		/// <summary>
		/// emergencyNumberPool.
		/// </summary>
		[EnumMember(Value = "emergencyNumberPool")]
		EmergencyNumberPool,

		/// <summary>
		/// companyLocation.
		/// </summary>
		[EnumMember(Value = "companyLocation")]
		CompanyLocation,

		/// <summary>
		/// meetingService.
		/// </summary>
		[EnumMember(Value = "meetingService")]
		MeetingService
	}
}
