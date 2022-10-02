using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration for the standard registration fields.
	/// </summary>
	public enum RegistrationField
	{
		/// <summary>
		/// Last name.
		/// </summary>
		[EnumMember(Value = "last_name")]
		LastName,

		/// <summary>
		/// Address.
		/// </summary>
		[EnumMember(Value = "address")]
		Address,

		/// <summary>
		/// City.
		/// </summary>
		[EnumMember(Value = "city")]
		City,

		/// <summary>
		/// Country.
		/// </summary>
		[EnumMember(Value = "country")]
		Country,

		/// <summary>
		/// Postal code / zip.
		/// </summary>
		[EnumMember(Value = "zip")]
		PostalCode,

		/// <summary>
		/// State.
		/// </summary>
		[EnumMember(Value = "state")]
		State,

		/// <summary>
		/// Phone.
		/// </summary>
		[EnumMember(Value = "phone")]
		Phone,

		/// <summary>
		/// Industry.
		/// </summary>
		[EnumMember(Value = "industry")]
		Industry,

		/// <summary>
		/// Organization.
		/// </summary>
		[EnumMember(Value = "org")]
		Organization,

		/// <summary>
		/// Job title.
		/// </summary>
		[EnumMember(Value = "job_title")]
		JobTitle,

		/// <summary>
		/// Purchasing time frame.
		/// </summary>
		[EnumMember(Value = "purchasing_time_frame")]
		PurchasingTimeFrame,

		/// <summary>
		/// Role in purchase process.
		/// </summary>
		[EnumMember(Value = "role_in_purchase_process")]
		RoleInPurchaseProcess,

		/// <summary>
		/// Number of employees.
		/// </summary>
		[EnumMember(Value = "no_of_employees")]
		NumberOfEmployees,

		/// <summary>
		/// Comments.
		/// </summary>
		[EnumMember(Value = "comments")]
		Comments
	}
}
