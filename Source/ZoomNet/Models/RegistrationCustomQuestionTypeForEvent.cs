using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of registration question for events.
	/// </summary>
	public enum RegistrationCustomQuestionTypeForEvent
	{
		/// <summary>Short.</summary>
		[EnumMember(Value = "short_answer")]
		ShortText,

		/// <summary>Long.</summary>
		[EnumMember(Value = "long_answer")]
		LongText,

		/// <summary>Single.</summary>
		[EnumMember(Value = "single_radio")]
		SingleRadio,

		/// <summary>Single.</summary>
		[EnumMember(Value = "single_dropdown")]
		SingleDropdown,

		/// <summary>Multiple choices.</summary>
		[EnumMember(Value = "multiple_choice")]
		MultipleChoices,
	}
}
