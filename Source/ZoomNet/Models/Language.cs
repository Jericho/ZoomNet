using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Language.
	/// </summary>
	public enum Language
	{
		/// <summary>Mandarin (PRC).</summary>
		[EnumMember(Value = "cmn-CN")]
		Mandarin_PRC,

		/// <summary>Danish (Denmark).</summary>
		[EnumMember(Value = "da-DK")]
		Danish_Denmark,

		/// <summary>German (Germany).</summary>
		[EnumMember(Value = "de-DE")]
		German_Germany,

		/// <summary>English (Australia).</summary>
		[EnumMember(Value = "en-AU")]
		English_Australia,

		/// <summary>English (UK).</summary>
		[EnumMember(Value = "en-GB")]
		English_UK,

		/// <summary>English (New Zealand).</summary>
		[EnumMember(Value = "en-NZ")]
		English_NewZealand,

		/// <summary>English (US).</summary>
		[EnumMember(Value = "en-US")]
		English_US,

		/// <summary>Spanish (Spain).</summary>
		[EnumMember(Value = "es-ES")]
		Spanish_Spain,

		/// <summary>Spanish (Mexico).</summary>
		[EnumMember(Value = "es-MX")]
		Spanish_Mexico,

		/// <summary>French (Canada).</summary>
		[EnumMember(Value = "fr-CA")]
		French_Canada,

		/// <summary>French (France).</summary>
		[EnumMember(Value = "fr-FR")]
		French_France,

		/// <summary>Italian (Italy).</summary>
		[EnumMember(Value = "it-IT")]
		Italian_Italy,

		/// <summary>Japanese (Japan).</summary>
		[EnumMember(Value = "ja-JP")]
		Japanese,

		/// <summary>Korean (Korea).</summary>
		[EnumMember(Value = "ko-KR")]
		Korean_Korea,

		/// <summary>Dutch (Netherlands).</summary>
		[EnumMember(Value = "nl-NL")]
		Dutch_Netherlands,

		/// <summary>Polish (Poland).</summary>
		[EnumMember(Value = "pl-PL")]
		Polish_Poland,

		/// <summary>Portuguese (Brazil).</summary>
		[EnumMember(Value = "pt-BR")]
		Portuguese_Brazil,

		/// <summary>Portuguese (Portugal).</summary>
		[EnumMember(Value = "pt-PT")]
		Portuguese_Portugal,

		/// <summary>Romanian (Romania).</summary>
		[EnumMember(Value = "ro-RO")]
		Romanian_Romania,

		/// <summary>Russian (Russia).</summary>
		[EnumMember(Value = "ru-RU")]
		Russian,

		/// <summary>Swedish (Sweden).</summary>
		[EnumMember(Value = "sv-SE")]
		Swedish_Sweden,

		/// <summary>Turkish (Turkey).</summary>
		[EnumMember(Value = "tr-TR")]
		Turkish_Turkey,

		/// <summary>Vietnamese.</summary>
		[EnumMember(Value = "vi-VN")]
		Vietnamese,

		/// <summary>Cantonese (PRC).</summary>
		[EnumMember(Value = "yue-CN")]
		Cantonese_PRC,

		/// <summary>Chinese (PRC).</summary>
		[EnumMember(Value = "zh-CN")]
		Chinese_PRC,

		/// <summary>Chinese (Hong Kong).</summary>
		[EnumMember(Value = "zh-HK")]
		Chinese_HongKong,

		/// <summary>Chinese (Taiwan).</summary>
		[EnumMember(Value = "zh-TW")]
		Chinese_Taiwan,
	}
}
