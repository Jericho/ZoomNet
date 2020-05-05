using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of poll question.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum QuestionType
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
		MultipleChoice
	}
}
