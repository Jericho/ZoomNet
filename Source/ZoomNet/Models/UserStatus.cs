using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a user.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum UserStatus
	{
		/// <summary>
		/// Active.
		/// </summary>
		[EnumMember(Value = "active")]
		Active,

		/// <summary>
		/// Inactive.
		/// </summary>
		[EnumMember(Value = "inactive")]
		Inactive,

		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending
	}
}
