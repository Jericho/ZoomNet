using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Registrant.
	/// </summary>
	public class Registrant
	{
		/// <summary>
		/// Gets or sets the registrant id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets a valid email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		[JsonPropertyName("address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		[JsonPropertyName("city")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		[JsonPropertyName("country")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the zip/postal Code.
		/// </summary>
		[JsonPropertyName("zip")]
		public string Zip { get; set; }

		/// <summary>
		/// Gets or sets the state/Province.
		/// </summary>
		[JsonPropertyName("state")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the phone.
		/// </summary>
		[JsonPropertyName("phone")]
		public string Phone { get; set; }

		/// <summary>
		/// Gets or sets the industry.
		/// </summary>
		[JsonPropertyName("industry")]
		public string Industry { get; set; }

		/// <summary>
		/// Gets or sets the organization.
		/// </summary>
		[JsonPropertyName("org")]
		public string Organization { get; set; }

		/// <summary>
		/// Gets or sets the job Title.
		/// </summary>
		[JsonPropertyName("job_title")]
		public string JobTitle { get; set; }

		/// <summary>
		/// Gets or sets the purchasing Time Frame.
		/// </summary>
		[JsonPropertyName("purchasing_time_frame")]
		public PurchasingTimeFrame PurchasingTimeFrame { get; set; }

		/// <summary>
		/// Gets or sets the role in purchase process.
		/// </summary>
		[JsonPropertyName("role_in_purchase_process")]
		public RoleInPurchaseProcess RoleInPurchaseProcess { get; set; }

		/// <summary>
		/// Gets or sets the number of employees.
		/// </summary>
		[JsonPropertyName("no_of_employees")]
		public NumberOfEmployees NumberOfEmployees { get; set; }

		/// <summary>
		/// Gets or sets the questions &amp; comments.
		/// </summary>
		[JsonPropertyName("comments")]
		public string Comments { get; set; }

		/// <summary>
		/// Gets or sets the custom questions.
		/// </summary>
		[JsonPropertyName("custom_questions")]
		public KeyValuePair<string, string>[] CustomQuestions { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		[JsonPropertyName("status")]
		public RegistrantStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the registrant was created.
		/// </summary>
		/// <value>The registrant created time.</value>
		[JsonPropertyName("created_time")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL for this registrant to join the meeting.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }
	}
}
