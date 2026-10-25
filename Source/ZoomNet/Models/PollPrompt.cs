using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Prompt question.</summary>
	public record PollPrompt
	{
		/// <summary>Gets or sets the question prompt's title.</summary>
		[JsonPropertyName("prompt_question")]
		public string Question { get; init; }

		/// <summary>Gets or sets the correct answers.</summary>
		[JsonPropertyName("prompt_right_answer")]
		public string[] CorrectAnswers { get; init; }
	}
}
