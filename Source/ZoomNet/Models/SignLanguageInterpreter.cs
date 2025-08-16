using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A sign language interpreter.
	/// </summary>
	public class SignLanguageInterpreter : Interpreter
	{
		/// <summary>
		/// Gets or sets the target language.
		/// </summary>
		[JsonPropertyName("target_language_id")]
		public InterpretationSignLanguage TargetLanguage { get; set; }

		/// <summary>Gets or sets the display name of the target language.</summary>
		[JsonPropertyName("target_language_display_name")]
		public string TargetLanguageDisplayName { get; set; }
	}
}
