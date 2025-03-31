using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An account's billing information.
	/// </summary>
	public class BillingInfo
	{
		/// <summary>
		/// Gets or sets the billing contact's address.
		/// </summary>
		[JsonPropertyName("address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's apartment or suite number.
		/// </summary>
		[JsonPropertyName("apt")]
		public string Apartment { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's city.
		/// </summary>
		[JsonPropertyName("city")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's country.
		/// </summary>
		[JsonPropertyName("country")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's first name.
		/// </summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's last name.
		/// </summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's phone number.
		/// </summary>
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's state.
		/// </summary>
		[JsonPropertyName("state")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the billing contact's zip or postal code.
		/// </summary>
		[JsonPropertyName("zip")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the range of employee count associated with the organization of this sub-account.
		/// </summary>
		[JsonPropertyName("employee_count")]
		public BillingEmployeeCount EmployeeCount { get; set; }
	}
}
