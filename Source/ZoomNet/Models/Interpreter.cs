using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A session interpreter.</summary>
	public abstract record Interpreter
	{
		/// <summary>Gets or sets the email address of the interpreter.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the interpreter type.</summary>
		[JsonPropertyName("type")]
		public InterpreterType Type { get; init; }
	}
}
