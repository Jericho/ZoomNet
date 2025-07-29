using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The answer to a question asked during a poll.
	/// </summary>
	public abstract class PollQuestion
	{
		/// <summary>Gets or sets the question asked during the poll.</summary>
		[JsonPropertyName("name")]
		public string Question { get; set; }

		/// <summary>Gets or sets the type of question.</summary>
		[JsonPropertyName("type")]
		public PollQuestionType Type { get; set; }

		/// <summary>Gets or sets a value indicating whether the possible answers will be displayed as a drop-down box.</summary>
		[JsonPropertyName("show_as_dropdown")]
		public bool? ShowAsDropdown { get; set; }

		/// <summary>Gets or sets a value indicating whether the question must be answered.</summary>
		[JsonPropertyName("answer_required")]
		public bool IsRequired { get; set; }

		/// <summary>Gets or sets the answers to the question.</summary>
		[JsonPropertyName("answers")]
		public string[] Answers { get; set; }

		/// <summary>Gets or sets the correct answer(s) to the question.</summary>
		[JsonPropertyName("right_answers")]
		public string[] CorrectAnswers { get; set; }

		/// <summary>Gets or sets the minimum number of characters.</summary>
		/// <remarks>
		/// This field only applies to questions of type 'Short' and 'Long'.
		/// Must be greather or equal to 1.
		/// </remarks>
		[JsonPropertyName("answer_min_character")]
		public int? MinimumNumberOfCharacters { get; set; }

		/// <summary>Gets or sets the maximum number of characters.</summary>
		/// <remarks>
		/// This field only applies to questions of type 'Short' and 'Long'.
		/// For short_answer polls, a maximum of 500 characters.
		/// For long_answer polls, a maximum of 2,000 characters.
		/// </remarks>
		[JsonPropertyName("answer_max_character")]
		public int? MaximumNumberOfCharacters { get; set; }

		/// <summary>Gets or sets a value indicating whether the corect answer is case sensitive.</summary>
		/// <remarks>This field only applies to questions of type 'Fill in the blanks'.</remarks>
		[JsonPropertyName("case_sensitive")]
		public bool? IsCaseSensitive { get; set; }

		/// <summary>Gets or sets the rating minimum value.</summary>
		/// <remarks>
		/// This field only applies to questions of type 'Rating'.
		/// Must be greather or equal to 0.
		/// </remarks>
		[JsonPropertyName("rating_min_value")]
		public int? RatingMinimumValue { get; set; }

		/// <summary>Gets or sets the rating maximum value.</summary>
		/// <remarks>
		/// This field only applies to questions of type 'Rating'.
		/// Must be smaller or equal to 10.
		/// </remarks>
		[JsonPropertyName("rating_max_value")]
		public int? RatingMaximumValue { get; set; }

		/// <summary>Gets or sets the low score label.</summary>
		/// <remarks>This field only applies to questions of type 'Rating'.</remarks>
		[JsonPropertyName("rating_min_label")]
		public string RatingLowScoreLabel { get; set; }

		/// <summary>Gets or sets the high score label.</summary>
		/// <remarks>This field only applies to questions of type 'Rating'.</remarks>
		[JsonPropertyName("rating_max_label")]
		public string RatingHighScoreLabel { get; set; }

		/// <summary>Gets or sets the question prompt's correct answers.</summary>
		/// <remarks>
		/// - For matching polls, you must provide a minimum of two correct answers, up to a maximum of 10 correct answers.
		/// - For rank_order polls, you can only provide one correct answer.
		/// </remarks>
		[JsonPropertyName("prompt_right_answers")]
		public string[] PromptCorrectAnswers { get; set; }

		/// <summary>Gets or sets the poll's title, up to 64 characters.</summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }
	}
}
