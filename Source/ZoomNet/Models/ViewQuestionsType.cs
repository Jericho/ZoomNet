using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the type of content in a recording file.</summary>
	public enum ViewQuestionsType
	{
		/// <summary>View answered questions only.</summary>
		[EnumMember(Value = "only")]
		ViewAnsweredQuestionsOnly,

		/// <summary>View all questions.</summary>
		[EnumMember(Value = "all")]
		ViewAllQuestions,
	}
}
