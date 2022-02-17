using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of registration question.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RegistrationCustomQuestionType
	{
		/// <summary>
		/// Short.
		/// </summary>
		[EnumMember(Value = "short")]
		Short,

		/// <summary>
		/// Single.
		/// </summary>
		[EnumMember(Value = "single")]
		Single
	}
}
