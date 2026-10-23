using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Email languages.</summary>
	public enum EmailLanguage
	{
		/// <summary>English (US).</summary>
		[EnumMember(Value = "en-US")]
		English_US,

		/// <summary>German (Germany).</summary>
		[EnumMember(Value = "de-DE")]
		German_Germany,

		/// <summary>Spanish (Spain).</summary>
		[EnumMember(Value = "es-ES")]
		Spanish_Spain,

		/// <summary>French (France).</summary>
		[EnumMember(Value = "fr-FR")]
		French_France,

		/// <summary>Indonesian (Indonesia).</summary>
		[EnumMember(Value = "id-ID")]
		Indonesian_Indonesia,

		/// <summary>Japanese (Japan).</summary>
		[EnumMember(Value = "jp-JP")]
		Japanese_Japan,

		/// <summary>Dutch (Netherlands).</summary>
		[EnumMember(Value = "nl-NL")]
		Dutch_Netherlands,

		/// <summary>Polish (Poland).</summary>
		[EnumMember(Value = "pl-PL")]
		Polish_Poland,

		/// <summary>Portuguese (Portugal).</summary>
		[EnumMember(Value = "pt-PT")]
		Portuguese_Portugal,

		/// <summary>Russian (Russia).</summary>
		[EnumMember(Value = "ru-RU")]
		Russian_Russia,

		/// <summary>Turkish (Turkey).</summary>
		[EnumMember(Value = "tr-TR")]
		Turkish_Turkey,

		/// <summary>Chinese (PRC).</summary>
		[EnumMember(Value = "zh-CN")]
		Chinese_PRC,

		/// <summary>Chinese (Taiwan).</summary>
		[EnumMember(Value = "zh-TW")]
		Chinese_Taiwan,

		/// <summary>Korean (Korea).</summary>
		[EnumMember(Value = "ko-KR")]
		Korean_Korea,

		/// <summary>Italian (Italy).</summary>
		[EnumMember(Value = "it-IT")]
		Italian_Italy,

		/// <summary>Vietnamese.</summary>
		[EnumMember(Value = "vi-VN")]
		Vietnamese_Vietnam,
	}
}
