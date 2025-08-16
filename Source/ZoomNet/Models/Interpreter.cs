using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A session interpreter.
	/// </summary>
	public abstract class Interpreter
	{
		/// <summary>Gets or sets the email address of the interpreter.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the interpreter type.
		/// </summary>
		[JsonPropertyName("type")]
		public InterpreterType Type { get; set; }
	}
}
