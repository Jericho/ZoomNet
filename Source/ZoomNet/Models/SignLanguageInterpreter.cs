using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>A sign language interpreter.</summary>
	[JsonConverter(typeof(InterpreterConverter))]
	public class SignLanguageInterpreter : Interpreter
	{
		/// <summary>Gets or sets the display name of the target language.</summary>
		[JsonPropertyName("target_language_display_name")]
		public string TargetLanguageDisplayName { get; set; }

		/// <summary>Gets or sets the target language.</summary>
		[JsonPropertyName("target_language_id")]
		public InterpretationSignLanguage TargetLanguage { get; set; }
	}
}
