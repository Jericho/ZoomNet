using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the type of the call.</summary>
	public enum CallLogCallType
	{
		/// <summary>Voice over IP.</summary>
		[EnumMember(Value = "voip")]
		Voip,

		/// <summary>Public Switched Telephone Network.</summary>
		[EnumMember(Value = "pstn")]
		Pstn,

		/// <summary>Toll free.</summary>
		[EnumMember(Value = "tollfree")]
		Tollfree,

		/// <summary>International.</summary>
		[EnumMember(Value = "international")]
		International,

		/// <summary>Contact center.</summary>
		[EnumMember(Value = "contactCenter")]
		ContactCenter
	}
}
