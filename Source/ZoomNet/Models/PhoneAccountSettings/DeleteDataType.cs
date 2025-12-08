using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// The data types to delete.
	/// </summary>
	public enum DeleteDataType
	{
		/// <summary>
		/// Call log data.
		/// </summary>
		[EnumMember(Value = "callLog")]
		CallLog,

		/// <summary>
		/// On demand recording data.
		/// </summary>
		[EnumMember(Value = "onDemandRecording")]
		OnDemandRecording,

		/// <summary>
		/// Automatic recording data.
		/// </summary>
		[EnumMember(Value = "automaticRecording")]
		AutomaticRecording,

		/// <summary>
		/// Voicemail data.
		/// </summary>
		[EnumMember(Value = "voicemail")]
		Voicemail,

		/// <summary>
		/// Videomail data.
		/// </summary>
		[EnumMember(Value = "videomail")]
		Videomail,

		/// <summary>
		/// SMS data.
		/// </summary>
		[EnumMember(Value = "sms")]
		Sms,
	}
}
