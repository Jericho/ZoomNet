using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Standard Registration question.
	/// </summary>
	public class RegistrationStandardQuestion
	{
		/// <summary>Gets or sets the field name.</summary>
		[JsonPropertyName("field_name")]
		public EventRegistrationField FieldName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a registrant must answer this question.
		/// </summary>
		[JsonPropertyName("required")]
		public bool IsRequired { get; set; }

		/// <summary>
		/// Gets or sets the title of the question.
		/// </summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>
		/// GEts or sets the unique identifier for the question.
		/// </summary>
		[JsonPropertyName("question_id")]
		public string Id { get; set; }
	}
}
