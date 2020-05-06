using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a registrant.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RegistrantStatus
	{
		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending,

		/// <summary>
		/// Approved.
		/// </summary>
		[EnumMember(Value = "approved")]
		Approved,

		/// <summary>
		/// Denied.
		/// </summary>
		[EnumMember(Value = "denied")]
		Denied
	}
}
