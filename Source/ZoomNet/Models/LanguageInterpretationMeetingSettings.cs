using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Language interpretation meeting settings.</summary>
	/// <typeparam name="T">The type of interpreter.</typeparam>
	public class LanguageInterpretationMeetingSettings<T>
		where T : Interpreter
	{
		/// <summary>Gets or sets a value indicating whether to enable language interpretation for the meeting.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the interpreters for the meeting.</summary>
		[JsonPropertyName("interpreters")]
		public T[] Interpreters { get; set; }
	}
}
