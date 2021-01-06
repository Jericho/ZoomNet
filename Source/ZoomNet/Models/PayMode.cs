using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate who the payee is.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PayMode
	{
		/// <summary>
		/// Master account holder pays.
		/// </summary>
		[EnumMember(Value = "master")]
		Master,

		/// <summary>
		/// Sub account holder pays.
		/// </summary>
		[EnumMember(Value = "sub")]
		SubAccount
	}
}
