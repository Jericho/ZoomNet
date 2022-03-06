using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of registration question for meetings.
	/// </summary>
	public enum RegistrationCustomQuestionTypeForMeeting
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
