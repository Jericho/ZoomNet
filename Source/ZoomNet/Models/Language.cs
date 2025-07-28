using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Language.
	/// </summary>
	public enum Language
	{
		/// <summary>English (UK).</summary>
		[EnumMember(Value = "en-GB")]
		English_UK,

		/// <summary>English (US).</summary>
		[EnumMember(Value = "en-US")]
		English_US,

		/// <summary>Arabic.</summary>
		[EnumMember(Value = "ar")]
		Arabic,

		/// <summary>Danish (Denmark).</summary>
		[EnumMember(Value = "da-DK")]
		Danish_Denmark,

		/// <summary>German (Germany).</summary>
		[EnumMember(Value = "de-DE")]
		German_Germany,

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

		/// <summary>Japanese.</summary>
		[EnumMember(Value = "ja")]
		Japanese,

		/// <summary>Korean (Korea).</summary>
		[EnumMember(Value = "ko-KR")]
		Korean_Korea,

		/// <summary>Dutch (Netherlands).</summary>
		[EnumMember(Value = "nl-NL")]
		Dutch_Netherlands,

		/// <summary>Portuguese (Brazil).</summary>
		[EnumMember(Value = "pt-BR")]
		Portuguese_Brazil,

		/// <summary>Portuguese (Portugal).</summary>
		[EnumMember(Value = "pt-PT")]
		Portuguese_Portugal,

		/// <summary>Russian.</summary>
		[EnumMember(Value = "ru")]
		Russian,

		/// <summary>Chinese (PRC).</summary>
		[EnumMember(Value = "zh-CN")]
		Chinese_PRC,

		/// <summary>Chinese (Hong Kong).</summary>
		[EnumMember(Value = "zh-HK")]
		Chinese_HongKong,
	}
}
