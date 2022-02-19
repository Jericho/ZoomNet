using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// A custom question to be answered during registration process for a webinar.
	/// </summary>
	public class RegistrationCustomQuestionForWebinar
	{
		/// <summary>
		/// Gets or sets the title of the custom question.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[JsonProperty("title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the type of question.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[JsonProperty("type")]
		public RegistrationCustomQuestionTypeForWebinar Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the registrant must answer this question.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[JsonProperty("required")]
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
		[JsonProperty("answers")]
		public string[] Answers { get; set; }
	}
}
