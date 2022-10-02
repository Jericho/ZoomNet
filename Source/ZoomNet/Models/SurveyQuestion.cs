using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Survey question.
	/// </summary>
	public class SurveyQuestion
	{
		/// <summary>
		/// Gets or sets the survey question.
		/// </summary>
		/// <remarks>Up to 255 characters.</remarks>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the question type.
		/// </summary>
		[JsonPropertyName("type")]
		public SurveyQuestionType Type { get; set; }

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
		/// Gets or sets the possible answers to chose from.
		/// </summary>
		[JsonPropertyName("answers")]
		public string[] Answers { get; set; }

		/// <summary>
		/// Gets or sets the minimum number of characters.
		/// This field only applies to questions of type 'Long'.
		/// </summary>
		/// <remarks>Must be greather or equal to 1.</remarks>
		[JsonPropertyName("answer_min_character")]
		public int? MinimumNumberOfCharacters { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of characters.
		/// This field only applies to questions of type 'Long'.
		/// </summary>
		/// <remarks>Must be smaller or equal to 2,000.</remarks>
		[JsonPropertyName("answer_max_character")]
		public int? MaximumNumberOfCharacters { get; set; }

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
