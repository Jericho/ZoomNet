using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Specifies the type of an address book contact's phone number.
	/// </summary>
	public enum ContactCenterAddressBookPhoneNumberType
	{
		/// <summary>
		/// Main.
		/// </summary>
		[EnumMember(Value = "main")]
		Main,

		/// <summary>
		/// Work.
		/// </summary>
		[EnumMember(Value = "work")]
		Work,

		/// <summary>
		/// Home.
		/// </summary>
		[EnumMember(Value = "home")]
		Home,

		/// <summary>
		/// Mobile.
		/// </summary>
		[EnumMember(Value = "mobile")]
		Mobile,

		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "other")]
		Other,
	}
}
