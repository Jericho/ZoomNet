using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the protocol.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Protocol
	{
		/// <summary>
		/// H.323.
		/// </summary>
		[EnumMember(Value = "H.323")]
		H323,

		/// <summary>
		/// SIP.
		/// </summary>
		[EnumMember(Value = "SIP")]
		SIP
	}
}
