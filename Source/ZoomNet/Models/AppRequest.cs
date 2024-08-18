using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A marketplace app request.
	/// </summary>
	public class AppRequest
	{
		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("request_user_id")]
		public string UserId { get; set; }

		/// <summary>Gets or sets the name of the user.</summary>
		[JsonPropertyName("request_user_name")]
		public string UserName { get; set; }

		/// <summary>Gets or sets the email of the user.</summary>
		[JsonPropertyName("request_user_email")]
		public string UserEmail { get; set; }

		/// <summary>Gets or sets the department of the user.</summary>
		[JsonPropertyName("request_user_department")]
		public string UserDepartment { get; set; }

		/// <summary>Gets or sets the date of the request.</summary>
		[JsonPropertyName("request_date_time")]
		public DateTime RequestDate { get; set; }

		/// <summary>Gets or sets the reason for the app requst.</summary>
		[JsonPropertyName("reason")]
		public string Reason { get; set; }

		/// <summary>Gets or sets the status of the app request.</summary>
		[JsonPropertyName("status")]
		public AppRequestStatus Status { get; set; }
	}
}
