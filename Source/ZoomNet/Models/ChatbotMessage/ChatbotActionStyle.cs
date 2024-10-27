using System.Runtime.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// Enumeration to indicate the style for the action.
/// </summary>
public enum ChatbotActionStyle
{
	/// <summary>
	/// Invalid. Do not use.
	/// </summary>
	Invalid,

	/// <summary>
	/// Members.
	/// </summary>
	[EnumMember(Value = "Primary")]
	Primary,

	/// <summary>
	/// Channels.
	/// </summary>
	[EnumMember(Value = "Update")]
	Update,

	/// <summary>
	/// Members.
	/// </summary>
	[EnumMember(Value = "Delete")]
	Delete,

	/// <summary>
	/// Channels.
	/// </summary>
	[EnumMember(Value = "Disabled")]
	Disabled
}
