using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Consumer status on receiving SMS messages.
	/// </summary>
	public enum SmsOptStatus
	{
		/// <summary>
		/// Consumer response to receive further SMS messages is pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending,

		/// <summary>
		/// Consumer opted out from receiving SMS messages.
		/// </summary>
		[EnumMember(Value = "opt_out")]
		OptOut,

		/// <summary>
		/// Consumer opted in to receive SMS messages.
		/// </summary>
		[EnumMember(Value = "opt_in")]
		OptIn,
	}
}
