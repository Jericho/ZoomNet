using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A custom question to be answered during registration process for an event.
	/// </summary>
	public class RegistrationCustomQuestionForEvent
	{
		/// <summary>Gets or sets the field name.</summary>
		[JsonPropertyName("field_name")]
		public string FieldName { get; set; }

		/// <summary>Gets or sets a value indicating whether a registrant must answer this question.</summary>
		[JsonPropertyName("required")]
		public bool IsRequired { get; set; }

		/// <summary>Gets or sets the title of the question.</summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>Gets or sets the unique identifier for the question.</summary>
		[JsonPropertyName("question_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the answer choices.</summary>
		[JsonPropertyName("options")]
		public string[] Options { get; set; }

		/// <summary>Gets or sets the type of the question.</summary>
		[JsonPropertyName("type")]
		public RegistrationCustomQuestionTypeForEvent Type { get; set; }

		/// <summary>Gets or sets the minimum length of the custom question answer.</summary>
		/// <remarks>
		/// This is applicable for short_answer/long_answer question types.
		/// If this custom question is required then the min_length should be atleast 1.
		/// </remarks>
		[JsonPropertyName("min_length")]
		public int? MinimumLength { get; set; }

		/// <summary>Gets or sets the maximum length of the custom question answer.</summary>
		/// <remarks>
		/// This is applicable for short_answer/long_answer question types.
		/// The max_length should be greater than or equal min_length.
		/// The allowed max_length for short_answer is 500 and for long_answer is 2000.
		/// </remarks>
		[JsonPropertyName("max_length")]
		public int? MaximumLength { get; set; }
	}
}
