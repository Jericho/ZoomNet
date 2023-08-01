using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of an action.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ActionType
	{
		/// <summary>
		/// Add.
		/// </summary>
		[EnumMember(Value = "add")]
		Add,

		/// <summary>
		/// Update.
		/// </summary>
		[EnumMember(Value = "update")]
		Update,

		/// <summary>
		/// Delete.
		/// </summary>
		[EnumMember(Value = "delete")]
		Delete
	}
}
