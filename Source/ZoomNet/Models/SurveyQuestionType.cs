using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of survey question.
	/// </summary>
	public enum SurveyQuestionType
	{
		/// <summary>
		/// Single choice.
		/// </summary>
		[EnumMember(Value = "single")]
		Single,

		/// <summary>
		/// Multiple choice.
		/// </summary>
		[EnumMember(Value = "multiple")]
		Multiple,

		/// <summary>
		/// Rating scale.
		/// </summary>
		[EnumMember(Value = "rating_scale")]
		Rating_Scale,

		/// <summary>
		/// Long answer.
		/// </summary>
		[EnumMember(Value = "long_answer")]
		Long,
	}
}
