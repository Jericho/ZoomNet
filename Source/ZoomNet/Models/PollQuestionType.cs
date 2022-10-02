using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of poll question.
	/// </summary>
	public enum PollQuestionType
	{
		/// <summary>
		/// Single.
		/// </summary>
		[EnumMember(Value = "single")]
		SingleChoice,

		/// <summary>
		/// Multiple.
		/// </summary>
		[EnumMember(Value = "multiple")]
		MultipleChoice,

		/// <summary>
		/// Matching.
		/// </summary>
		[EnumMember(Value = "matching")]
		Matching,

		/// <summary>
		/// Rank order.
		/// </summary>
		[EnumMember(Value = "rank_order")]
		RankOrder,

		/// <summary>
		/// Short answer.
		/// </summary>
		[EnumMember(Value = "short_answer")]
		Short,

		/// <summary>
		/// Long answer.
		/// </summary>
		[EnumMember(Value = "long_answer")]
		Long,

		/// <summary>
		/// Fill in the blanks.
		/// </summary>
		[EnumMember(Value = "fill_in_the_blanks")]
		FillInTheBlanks,

		/// <summary>
		/// Rating scale.
		/// </summary>
		[EnumMember(Value = "rating_scale")]
		RatingScale
	}
}
