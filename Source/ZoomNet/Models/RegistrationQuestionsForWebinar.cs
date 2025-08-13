namespace ZoomNet.Models
{
	/// <summary>
	/// The questions on the registration form.
	/// </summary>
	public class RegistrationQuestionsForWebinar
	{
		/// <summary>Gets or sets the required fields.</summary>
		public RegistrationField[] RequiredFields { get; set; }

		/// <summary>Gets or sets the optional fields.</summary>
		public RegistrationField[] OptionalFields { get; set; }

		/// <summary>Gets or sets the custom questions.</summary>
		public RegistrationCustomQuestionForWebinar[] Questions { get; set; }
	}
}
