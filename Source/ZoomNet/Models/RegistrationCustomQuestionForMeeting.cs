using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A custom question to be answered during registration process for a meeting.
	/// </summary>
	public class RegistrationCustomQuestionForMeeting
	{
		/// <summary>
		/// Gets or sets the title of the custom question.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the type of question.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[JsonPropertyName("type")]
		public RegistrationCustomQuestionTypeForMeeting Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the registrant must answer this question.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[JsonPropertyName("required")]
		public bool IsRequired { get; set; }

		/// <summary>
		/// Gets or sets the answers to the question.
		/// </summary>
		/// <remarks>
		/// Can not be used for short question type as this type of question requires registrants to type out the answer.
		/// </remarks>
		/// <value>
		/// The answers.
		/// </value>
		[JsonPropertyName("answers")]
		public string[] Answers { get; set; }
	}
}
