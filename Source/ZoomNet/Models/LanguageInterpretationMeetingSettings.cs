using System.Text.Json.Serialization;
using ZoomNet.Json;

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
		[JsonConverter(typeof(InterpretersConverter<>))] // Open generics support is a new feature in .NET 11 (C# 14)
		public T[] Interpreters { get; set; }
	}
}
