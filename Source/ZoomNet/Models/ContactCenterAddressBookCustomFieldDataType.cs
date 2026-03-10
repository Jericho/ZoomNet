using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Specifies the data type of an address book custom field.
	/// </summary>
	public enum ContactCenterAddressBookCustomFieldDataType
	{
		/// <summary>
		/// Main.
		/// </summary>
		[EnumMember(Value = "string")]
		String,

		/// <summary>
		/// Work.
		/// </summary>
		[EnumMember(Value = "number")]
		Number,

		/// <summary>
		/// Home.
		/// </summary>
		[EnumMember(Value = "boolean")]
		Boolean,

		/// <summary>
		/// Mobile.
		/// </summary>
		[EnumMember(Value = "email")]
		Email,

		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "phone")]
		Phone,

		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "percent")]
		Percent,

		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "currency")]
		Currency,

		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "dateTime")]
		DateTime,

		/// <summary>
		/// Other.
		/// </summary>
		[EnumMember(Value = "pick_list")]
		PickList,
	}
}
