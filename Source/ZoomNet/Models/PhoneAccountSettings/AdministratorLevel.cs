using System.Runtime.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Administrator level that prohibits modification of the current settings.
	/// </summary>
	public enum AdministratorLevel
	{
		/// <summary>
		/// Invalid.
		/// </summary>
		[EnumMember(Value = "invalid")]
		Invalid,

		/// <summary>
		/// Account.
		/// </summary>
		[EnumMember(Value = "account")]
		Account,

		/// <summary>
		/// User group.
		/// </summary>
		[EnumMember(Value = "user_group")]
		UserGroup,

		/// <summary>
		/// Site.
		/// </summary>
		[EnumMember(Value = "site")]
		Site,
	}
}
