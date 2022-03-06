using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Language interpretation settings.
	/// </summary>
	public class LanguageInterpretationSettings
	{
		/// <summary>Gets or sets the supported user-defined languages.</summary>
		[JsonPropertyName("custom_languages")]
		public string[] SupportedCustomLanguages { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow hosts to assign participants as interpreters who can interpret one language into another in real-time.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the supported system languages.</summary>
		[JsonPropertyName("languages")]
		public InterpretationLanguage[] SupportedLanguages { get; set; }
	}
}
