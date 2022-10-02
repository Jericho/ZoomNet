using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The answer to a question asked during a poll.
	/// </summary>
	public class PollQuestion
	{
		/// <summary>
		/// Gets or sets the question asked during the poll.
		/// </summary>
		/// <value>
		/// The question.
		/// </value>
		[JsonPropertyName("name")]
		public string Question { get; set; }

		/// <summary>
		/// Gets or sets the type of question.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[JsonPropertyName("type")]
		public PollQuestionType Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the possible answers will be displayed as a drop-down box.
		/// </summary>
		[JsonPropertyName("show_as_dropdown")]
		public bool ShowAsDropdown { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the question must be answered.
		/// </summary>
		[JsonPropertyName("answer_required")]
		public bool IsRequired { get; set; }

		/// <summary>
		/// Gets or sets the answers to the question.
		/// </summary>
		/// <value>
		/// The answers.
		/// </value>
		[JsonPropertyName("answer")]
		public string[] Answers { get; set; }

		/// <summary>
		/// Gets or sets the correct answer(s) to the question.
		/// </summary>
		/// <value>
		/// The answers.
		/// </value>
		[JsonPropertyName("right_answers")]
		public string[] CorrectAnswers { get; set; }

		/// <summary>
		/// Gets or sets the information about the prompt questions.
		/// This field only applies to questions of type 'Matching' and 'Rank'.
		/// </summary>
		/// <remarks>You must provide at least two prompts and no more than 10 prompts.</remarks>
		[JsonPropertyName("prompts")]
		public PollPrompt[] Prompts { get; set; }

		/// <summary>
		/// Gets or sets the minimum number of characters.
		/// This field only applies to questions of type 'Short' and 'Long'.
		/// </summary>
		/// <remarks>Must be greather or equal to 1.</remarks>
		[JsonPropertyName("answer_min_character")]
		public int? MinimumNumberOfCharacters { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of characters.
		/// This field only applies to questions of type 'Short' and 'Long'.
		/// </summary>
		/// <remarks>Must be smaller or equal to 2,000.</remarks>
		[JsonPropertyName("answer_max_character")]
		public int? MaximumNumberOfCharacters { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the corect answer is case sensitive.
		/// This field only applies to questions of type 'Fill in the blanks'.
		/// </summary>
		[JsonPropertyName("case_sensitive")]
		public bool? IsCaseSensitive { get; set; }

		/// <summary>
		/// Gets or sets the rating minimum value.
		/// This field only applies to questions of type 'Rating'.
		/// </summary>
		/// <remarks>Must be greather or equal to 0.</remarks>
		[JsonPropertyName("rating_min_value")]
		public int? RatingMinimumValue { get; set; }

		/// <summary>
		/// Gets or sets the rating maximum value.
		/// This field only applies to questions of type 'Rating'.
		/// </summary>
		/// <remarks>Must be smaller or equal to 10.</remarks>
		[JsonPropertyName("rating_max_value")]
		public int? RatingMaximumValue { get; set; }

		/// <summary>
		/// Gets or sets the low score label.
		/// This field only applies to questions of type 'Rating'.
		/// </summary>
		[JsonPropertyName("rating_min_label")]
		public string RatingLowScoreLabel { get; set; }

		/// <summary>
		/// Gets or sets the high score label.
		/// This field only applies to questions of type 'Rating'.
		/// </summary>
		[JsonPropertyName("rating_max_label")]
		public string RatingHighScoreLabel { get; set; }
	}
}
