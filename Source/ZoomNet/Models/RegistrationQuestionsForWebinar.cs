namespace ZoomNet.Models
{
	/// <summary>
	/// The questions on the registration form.
	/// </summary>
	public class RegistrationQuestionsForWebinar
	{
		/// <summary>
		/// Gets or sets the required fields.
		/// </summary>
		/// <value>
		/// The required fields.
		/// </value>
		public RegistrationField[] RequiredFields { get; set; }

		/// <summary>
		/// Gets or sets the optional fields.
		/// </summary>
		/// <value>
		/// The optional fields.
		/// </value>
		public RegistrationField[] OptionalFields { get; set; }

		/// <summary>
		/// Gets or sets the custom questions.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public RegistrationCustomQuestionForWebinar[] Questions { get; set; }
	}
}
