using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of registration question for webinars.
	/// </summary>
	public enum RegistrationCustomQuestionTypeForWebinar
	{
		/// <summary>
		/// Short.
		/// </summary>
		[EnumMember(Value = "short")]
		Short,

		/// <summary>
		/// Single.
		/// </summary>
		[EnumMember(Value = "single_radio")]
		SingleRadio,

		/// <summary>
		/// Single.
		/// </summary>
		[EnumMember(Value = "single_dropdown")]
		SingleDropdown,

		/// <summary>
		/// Single.
		/// </summary>
		[EnumMember(Value = "multiple")]
		Multiple,
	}
}
