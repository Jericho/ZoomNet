using Newtonsoft.Json;
using ZoomNet.Models;
using System;
using System.Collections.Generic;

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
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets a valid email address.
		/// </summary>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[JsonProperty(PropertyName = "first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonProperty(PropertyName = "last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		[JsonProperty(PropertyName = "address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		[JsonProperty(PropertyName = "city")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		[JsonProperty(PropertyName = "country")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the zip/postal Code.
		/// </summary>
		[JsonProperty(PropertyName = "zip")]
		public string Zip { get; set; }

		/// <summary>
		/// Gets or sets the state/Province.
		/// </summary>
		[JsonProperty(PropertyName = "state")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the phone.
		/// </summary>
		[JsonProperty(PropertyName = "phone")]
		public string Phone { get; set; }

		/// <summary>
		/// Gets or sets the industry.
		/// </summary>
		[JsonProperty(PropertyName = "industry")]
		public string Industry { get; set; }

		/// <summary>
		/// Gets or sets the organization.
		/// </summary>
		[JsonProperty(PropertyName = "org")]
		public string Organization { get; set; }

		/// <summary>
		/// Gets or sets the job Title.
		/// </summary>
		[JsonProperty(PropertyName = "job_title")]
		public string JobTitle { get; set; }

		/// <summary>
		/// Gets or sets the purchasing Time Frame.
		/// </summary>
		[JsonProperty(PropertyName = "purchasing_time_frame")]
		public string PurchasingTimeFrame { get; set; }

		/// <summary>
		/// Gets or sets the role in purchase process.
		/// </summary>
		[JsonProperty(PropertyName = "role_in_purchase_process")]
		public string RoleInPurchaseProcess { get; set; }

		/// <summary>
		/// Gets or sets the number of employees.
		/// </summary>
		[JsonProperty(PropertyName = "no_of_employees")]
		public string NumberOfEmployees { get; set; }

		/// <summary>
		/// Gets or sets the questions &amp; comments.
		/// </summary>
		[JsonProperty(PropertyName = "comments")]
		public string Comments { get; set; }

		/// <summary>
		/// Gets or sets the custom questions.
		/// </summary>
		[JsonProperty(PropertyName = "custom_questions")]
		public KeyValuePair<string, string>[] CustomQuestions { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		[JsonProperty(PropertyName = "comments")]
		public RegistrantStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the registrant was created.
		/// </summary>
		/// <value>The registrant created time.</value>
		[JsonProperty(PropertyName = "created_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the URL for this registrant to join the meeting.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonProperty(PropertyName = "join_url", NullValueHandling = NullValueHandling.Ignore)]
		public string JoinUrl { get; set; }
	}
}
