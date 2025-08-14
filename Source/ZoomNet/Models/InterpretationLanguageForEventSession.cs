using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Interpretation language for an event session.
	/// </summary>
	public enum InterpretationLanguageForEventSession
	{
		/// <summary>English.</summary>
		[EnumMember(Value = "US")]
		English,

		/// <summary>Chinese.</summary>
		[EnumMember(Value = "CN")]
		Chinese,

		/// <summary>Japanese.</summary>
		[EnumMember(Value = "JP")]
		Japanese,

		/// <summary>German.</summary>
		[EnumMember(Value = "DE")]
		German,

		/// <summary>French.</summary>
		[EnumMember(Value = "FR")]
		French,

		/// <summary>Russian.</summary>
		[EnumMember(Value = "RU")]
		Russian,

		/// <summary>Portuguese.</summary>
		[EnumMember(Value = "PT")]
		Portuguese,

		/// <summary>Spanish.</summary>
		[EnumMember(Value = "ES")]
		Spanish,

		/// <summary>Korean.</summary>
		[EnumMember(Value = "KR")]
		Korean
	}
}
