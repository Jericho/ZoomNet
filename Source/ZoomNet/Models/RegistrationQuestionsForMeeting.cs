namespace ZoomNet.Models
{
	/// <summary>
	/// The questions on the registration form.
	/// </summary>
	public class RegistrationQuestionsForMeeting
	{
		/// <summary>Gets or sets the required fields.</summary>
		public RegistrationField[] RequiredFields { get; set; }

		/// <summary>Gets or sets the optional fields.</summary>
		public RegistrationField[] OptionalFields { get; set; }

		/// <summary>Gets or sets the custom questions.</summary>
		public RegistrationCustomQuestionForMeeting[] Questions { get; set; }
	}
}
