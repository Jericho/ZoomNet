using System.Runtime.Serialization;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// Enumeration to indicate the type of static source to populate a dropdown list.
/// </summary>
public enum ChatbotDropdownStaticSource
{
	/// <summary>
	/// Unspecified. Do not use.
	/// </summary>
	Unspecified,

	/// <summary>
	/// Members.
	/// </summary>
	[EnumMember(Value = "members")]
	Members,

	/// <summary>
	/// Channels.
	/// </summary>
	[EnumMember(Value = "channels")]
	Channels
}
