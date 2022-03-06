using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of audio available to attendees.
	/// </summary>
	public enum AudioType
	{
		/// <summary>
		/// VOIP only.
		/// </summary>
		[EnumMember(Value = "voip")]
		Voip,

		/// <summary>
		/// PSTN telephony only.
		/// </summary>
		[EnumMember(Value = "telephony")]
		Telephony,

		/// <summary>
		/// Both telephony and VOIP.
		/// </summary>
		[EnumMember(Value = "both")]
		Both,

		/// <summary>
		/// Third party audio conference.
		/// </summary>
		[EnumMember(Value = "thirdParty")]
		ThirdParty
	}
}
