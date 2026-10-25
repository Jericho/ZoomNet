namespace ZoomNet.Models
{
	/// <summary>The questions on the registration form.</summary>
	public record RegistrationQuestionsForMeeting
	{
		/// <summary>Gets or sets the required fields.</summary>
		public RegistrationField[] RequiredFields { get; init; }

		/// <summary>Gets or sets the optional fields.</summary>
		public RegistrationField[] OptionalFields { get; init; }

		/// <summary>Gets or sets the custom questions.</summary>
		public RegistrationCustomQuestionForMeeting[] Questions { get; init; }
	}
}
