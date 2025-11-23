using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The result reason of an event for a call log.
	/// </summary>
	public enum CallElementResultReason
	{
		/// <summary>Answered by other.</summary>
		[EnumMember(Value = "answered_by_other")]
		AnsweredByOther,

		/// <summary>Picked up by other.</summary>
		[EnumMember(Value = "pickup_by_other")]
		PickupByOther,

		/// <summary>Called out by other.</summary>
		[EnumMember(Value = "call_out_by_other")]
		CallOutByOther,
	}
}
