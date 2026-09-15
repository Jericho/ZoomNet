using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the participants of the waiting room.</summary>
	public enum MeetingWaitingRoomParticipants
	{
		/// <summary>Everyone.</summary>
		[EnumMember(Value = "everyone")]
		Everyone,

		/// <summary>Users not in your account.</summary>
		[EnumMember(Value = "users_not_in_account")]
		UsersNotInAccount,

		/// <summary>Users not in your account or whitelisted domains.</summary>
		[EnumMember(Value = "users_not_in_account_or_whitelisted_domains")]
		UsersNotInAccountOrWhitelistedDomains,

		/// <summary>Users not on the invite list.</summary>
		[EnumMember(Value = "users_not_on_invite")]
		UsersNotOnInvite,

		/// <summary>Users not in your organization.</summary>
		[EnumMember(Value = "users_not_in_org")]
		UsersNotInOrg,
	}
}
