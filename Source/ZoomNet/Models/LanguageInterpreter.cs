using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A language interpreter.
	/// </summary>
	public class LanguageInterpreter : Interpreter
	{
		/// <summary>
		/// Gets or sets the source language.
		/// </summary>
		[JsonPropertyName("source_language_id")]
		public InterpretationLanguageForEventSession SourceLanguage { get; set; }

		/// <summary>Gets or sets the display name of the source language.</summary>
		[JsonPropertyName("source_language_display_name")]
		public string SourceLanguageDisplayName { get; set; }

		/// <summary>
		/// Gets or sets the target language.
		/// </summary>
		[JsonPropertyName("target_language_id")]
		public InterpretationLanguageForEventSession TargetLanguage { get; set; }

		/// <summary>Gets or sets the display name of the target language.</summary>
		[JsonPropertyName("target_language_display_name")]
		public string TargetLanguageDisplayName { get; set; }
	}
}
