using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of audio available to attendees.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MeetingAudioType
	{
		/// <summary>
		/// VOIP.
		/// </summary>
		[EnumMember(Value = "voip")]
		Voip,

		/// <summary>
		/// Telephony.
		/// </summary>
		[EnumMember(Value = "telephony")]
		Telephony,

		/// <summary>
		/// Both telephony and voip.
		/// </summary>
		[EnumMember(Value = "both")]
		Both
	}
}
