using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Asset language codes.
	/// </summary>
	public enum AssetLanguage
	{
		/// <summary>Mandarin (China).</summary>
		[EnumMember(Value = "cmn-CN")]
		Mandarin_China,

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
		English_UnitedKingdom,

		/// <summary>English (New Zealand).</summary>
		[EnumMember(Value = "en-NZ")]
		English_NewZealand,

		/// <summary>English (US).</summary>
		[EnumMember(Value = "en-US")]
		English_UnitedStates,

		/// <summary>Spanish (Spain).</summary>
		[EnumMember(Value = "es-ES")]
		Spanish_Spain,

		/// <summary>Spanish (Mexico).</summary>
		[EnumMember(Value = "es-MX")]
		Spanish_Mexico,

		/// <summary>Spanish (US).</summary>
		[EnumMember(Value = "es-US")]
		Spanish_UnitedStates,

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
		Japanese_Japan,

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
		Russian_Russia,

		/// <summary>Swedish (Sweden).</summary>
		[EnumMember(Value = "sv-SE")]
		Swedish_Sweden,

		/// <summary>Turkish (Turkey).</summary>
		[EnumMember(Value = "tr-TR")]
		Turkish_Turkey,

		/// <summary>Cantonese (China).</summary>
		[EnumMember(Value = "yue-CN")]
		Cantonese_China,

		/// <summary>Chinese (China).</summary>
		[EnumMember(Value = "zh-CN")]
		Chinese_China,

		/// <summary>Chinese (Taiwan).</summary>
		[EnumMember(Value = "zh-TW")]
		Chinese_Taiwan,
	}
}
