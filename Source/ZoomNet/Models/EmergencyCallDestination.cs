using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Emergency call routing destination.
	/// </summary>
	public enum EmergencyCallDestination
	{
		/// <summary>
		/// PSAP (Public safety answering point).
		/// </summary>
		[EnumMember(Value = "PSAP")]
		Psap,

		/// <summary>
		/// Internal safety response team.
		/// </summary>
		[EnumMember(Value = "SAFETY_TEAM")]
		SafetyTeam,

		/// <summary>
		/// None.
		/// </summary>
		[EnumMember(Value = "NONE")]
		None,

		/// <summary>
		/// Both PSAP and safety team.
		/// </summary>
		[EnumMember(Value = "BOTH")]
		BothPsapAndSafetyTeam,

		/// <summary>
		/// Mobile 911 call.
		/// </summary>
		[EnumMember(Value = "MOBILE_911_CALL")]
		Mobile911Call,

		/// <summary>
		/// SIP group.
		/// </summary>
		[EnumMember(Value = "SIP_GROUP")]
		SipGroup,

		/// <summary>
		/// Both safety team and SIP group.
		/// </summary>
		[EnumMember(Value = "BOTH_SAFETY_TEAM_AND_SIP")]
		BothSafetyTeamAndSip,

		/// <summary>
		/// SIP group due to overflow.
		/// </summary>
		[EnumMember(Value = "OVERFLOW_TO_SIP_GROUP")]
		OverflowToSipGroup,

		/// <summary>
		/// PSAP due to missed by safety team.
		/// </summary>
		[EnumMember(Value = "TO_PSAP_FOR_MISSED_BY_SAFETY_TEAM")]
		PsapMissedBySafetyTeam,
	}
}
