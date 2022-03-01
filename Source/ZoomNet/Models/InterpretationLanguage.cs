using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Interpretation language.
	/// </summary>
	public enum InterpretationLanguage
	{
		/// <summary>English</summary>
		[EnumMember(Value = "English")]
		English,

		/// <summary>Chinese</summary>
		[EnumMember(Value = "Chinese")]
		Chinese,

		/// <summary>Japanese</summary>
		[EnumMember(Value = "Japanese")]
		Japanese,

		/// <summary>German</summary>
		[EnumMember(Value = "German")]
		German,

		/// <summary>French</summary>
		[EnumMember(Value = "French")]
		French,

		/// <summary>Russian</summary>
		[EnumMember(Value = "Russian")]
		Russian,

		/// <summary>Portuguese</summary>
		[EnumMember(Value = "Portuguese")]
		Portuguese,

		/// <summary>Spanish</summary>
		[EnumMember(Value = "Spanish")]
		Spanish,

		/// <summary>Korean</summary>
		[EnumMember(Value = "Korean")]
		Korean
	}
}
