using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the role of a member of a chat channel.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ChatChannelRole
	{
		/// <summary>
		/// Live.
		/// </summary>
		[EnumMember(Value = "owner")]
		Owner,

		/// <summary>
		/// Past
		/// </summary>
		[EnumMember(Value = "admin")]
		Administrator,

		/// <summary>
		/// PastOne
		/// </summary>
		[EnumMember(Value = "member")]
		Member
	}
}
