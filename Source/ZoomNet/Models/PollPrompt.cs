using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Prompt question.
	/// </summary>
	public class PollPrompt
	{
		/// <summary>
		/// Gets or sets the question prompt's title.
		/// </summary>
		[JsonProperty("prompt_question")]
		public string Question { get; set; }

		/// <summary>
		/// Gets or sets the correct answers.
		/// </summary>
		[JsonProperty("prompt_right_answer")]
		public string[] CorrectAnswers { get; set; }
	}
}
