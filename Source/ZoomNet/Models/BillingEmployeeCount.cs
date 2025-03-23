using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the range of employee count associated with an organization.
	/// </summary>
	public enum BillingEmployeeCount
	{
		/// <summary>One.</summary>
		[EnumMember(Value = "")]
		[Display(Order = 0)]
		Unkown,

		/// <summary>One.</summary>
		[EnumMember(Value = "Just Me")]
		[Display(Order = 1)]
		One,

		/// <summary>Between 2 and 10.</summary>
		[EnumMember(Value = "2-10")]
		[Display(Order = 2)]
		Between2And10,

		/// <summary>Between 11 and 50.</summary>
		[EnumMember(Value = "11-50")]
		[Display(Order = 3)]
		Between11And50,

		/// <summary>Between 51 and 250.</summary>
		[EnumMember(Value = "51-250")]
		[Display(Order = 4)]
		Between51And250,

		/// <summary>Between 251 and 500.</summary>
		[EnumMember(Value = "251-500")]
		[Display(Order = 5)]
		Between251And500,

		/// <summary>Between 501 and 1000.</summary>
		[EnumMember(Value = "501-1000")]
		[Display(Order = 6)]
		Between501And1000,

		/// <summary>Between 1001 and 5000.</summary>
		[EnumMember(Value = "1001-5000")]
		[Display(Order = 7)]
		Between1001And5000,

		/// <summary>Between 251 and 500.</summary>
		[EnumMember(Value = "5001-10000")]
		[Display(Order = 8)]
		Between5001And10000,

		/// <summary>More than 10000.</summary>
		[EnumMember(Value = "10001+")]
		[Display(Order = 9)]
		MoreThan10001,
	}
}
