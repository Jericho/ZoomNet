using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration for the standard registration fields for an event.
	/// </summary>
	public enum EventRegistrationField
	{
		/// <summary>Address.</summary>
		[EnumMember(Value = "address")]
		Address,

		/// <summary>City.</summary>
		[EnumMember(Value = "city")]
		City,

		/// <summary>State / Province.</summary>
		[EnumMember(Value = "state")]
		State,

		/// <summary>Postal code / zip.</summary>
		[EnumMember(Value = "zip")]
		PostalCode,

		/// <summary>Country.</summary>
		[EnumMember(Value = "country")]
		Country,

		/// <summary>Phone.</summary>
		[EnumMember(Value = "phone")]
		Phone,

		/// <summary>Industry.</summary>
		[EnumMember(Value = "industry")]
		Industry,

		/// <summary>Job title.</summary>
		[EnumMember(Value = "job_title")]
		JobTitle,

		/// <summary>Organization.</summary>
		[EnumMember(Value = "organization")]
		Organization,

		/// <summary>Organization size.</summary>
		[EnumMember(Value = "organization_size")]
		OrganizationSize,
	}
}
