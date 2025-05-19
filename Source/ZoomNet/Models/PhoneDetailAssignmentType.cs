using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Specifies the assignment status for a phone number in the Zoom Phone API.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// <item><term>assigned</term><description>Number has been assigned to a user, call queue, auto-receptionist, or common area.</description></item>
	/// <item><term>unassigned</term><description>Number is not assigned to anyone.</description></item>
	/// <item><term>all</term><description>Include both assigned and unassigned numbers in the response.</description></item>
	/// <item><term>byoc</term><description>Include Bring Your Own Carrier (BYOC) numbers only in the response.</description></item>
	/// </list>
	/// </remarks>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PhoneDetailAssignmentType
	{
		/// <summary>
		/// Number has been assigned to a user, call queue, auto-receptionist, or common area.
		/// </summary>
		assigned,

		/// <summary>
		/// Number is not assigned to anyone.
		/// </summary>
		unassigned,

		/// <summary>
		/// Include both assigned and unassigned numbers in the response.
		/// </summary>
		all,

		/// <summary>
		/// Include Bring Your Own Carrier (BYOC) numbers only in the response.
		/// </summary>
		byoc
	}
}
