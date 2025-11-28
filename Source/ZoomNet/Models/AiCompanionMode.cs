using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of AI companion.
	/// </summary>
	public enum AiCompanionMode
	{
		/// <summary>
		/// The AI Companion for answering questions.
		/// </summary>
		[EnumMember(Value = "questions")]
		Questions,

		/// <summary>
		/// The AI Companion for generating meeting summaries.
		/// </summary>
		[EnumMember(Value = "summary")]
		Summary,

		/// <summary>
		/// Both modes.
		/// </summary>
		[EnumMember(Value = "all")]
		All,
	}
}
