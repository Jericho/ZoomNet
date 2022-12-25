using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of an app.
	/// </summary>
	public enum AppType
	{
		/// <summary>A Zoom app.</summary>
		[EnumMember(Value = "ZoomApp")]
		ZoomApp,

		/// <summary>A ChatBot app.</summary>
		[EnumMember(Value = "ChatBotApp")]
		ChatBotApp,

		/// <summary>An OAuth app.</summary>
		[EnumMember(Value = "OAuthApp")]
		OAuthApp,
	}
}
