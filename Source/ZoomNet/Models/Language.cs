using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Language.
	/// </summary>
	public enum Language
	{
		/// <summary>English (US)</summary>
		[EnumMember(Value = "en-US")]
		English_US,

		/// <summary>German (Germany)</summary>
		[EnumMember(Value = "de-DE")]
		German_Germany,

		/// <summary>Spanish (Spain)</summary>
		[EnumMember(Value = "es-ES")]
		Spanish_Spain,

		/// <summary>French (France)</summary>
		[EnumMember(Value = "fr-FR")]
		French_France,

		/// <summary> Japanese</summary>
		[EnumMember(Value = "jp-JP")]
		Japanese,

		/// <summary>Portuguese (Portugal)</summary>
		[EnumMember(Value = "pt-PT")]
		Portuguese_Portugal,

		/// <summary>Russian</summary>
		[EnumMember(Value = "ru-RU")]
		Russian,

		/// <summary>Chinese (PRC)</summary>
		[EnumMember(Value = "zh-CN")]
		Chinese_PRC,

		/// <summary>Chinese (Taiwan)</summary>
		[EnumMember(Value = "zh-TW")]
		Chinese_Taiwan,

		/// <summary>Korean</summary>
		[EnumMember(Value = "ko-KO")]
		Korean,

		/// <summary>Italian (Italy)</summary>
		[EnumMember(Value = "it-IT")]
		Italian_Italy,

		/// <summary>Vietnamese</summary>
		[EnumMember(Value = "vi-VN")]
		Vietnamese,

		/// <summary>Polish</summary>
		[EnumMember(Value = "pl-PL")]
		Polish,

		/// <summary>Turkish</summary>
		[EnumMember(Value = "Tr-TR")]
		Turkish,
	}
}
