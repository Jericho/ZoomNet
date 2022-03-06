using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// User.
	/// </summary>
	public class User
	{
		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the date and time when the user was created.</summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>Gets or sets the department.</summary>
		[JsonPropertyName("dept")]
		public string Department { get; set; }

		/// <summary>Gets or sets a valid email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>Gets or sets the last login client version.</summary>
		[JsonPropertyName("last_client_version")]
		public string LastLoginClientVersion { get; set; }

		/// <summary>Gets or sets the date and time when the user last logged in.</summary>
		[JsonPropertyName("last_login_time")]
		public DateTime LastLogin { get; set; }

		/// <summary>Gets or sets the last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>Gets or sets the personal meeting id.</summary>
		[JsonPropertyName("pmi")]
		public long PersonalMeetingId { get; set; }

		/// <summary>Gets or sets the role name.</summary>
		[JsonPropertyName("role_name")]
		public string RoleName { get; set; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public string Timezone { get; set; }

		/// <summary>Gets or sets the type.</summary>
		[JsonPropertyName("type")]
		public UserType Type { get; set; }

		/// <summary>Gets or sets a value indicating whether the PMI is used for instant meetings.</summary>
		[JsonPropertyName("use_pmi")]
		public bool UsePersonalMeetingId { get; set; }

		/// <summary>Gets or sets the account id.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>Gets or sets the account number.</summary>
		[JsonPropertyName("account_number")]
		public long AccountNumber { get; set; }

		/// <summary>Gets or sets the CMS ID.</summary>
		/// <remarks>Only enabled for Kaltura integration.</remarks>
		[JsonPropertyName("cms_user_id")]
		public string CmsUserId { get; set; }

		/// <summary>Gets or sets the company.</summary>
		[JsonPropertyName("company")]
		public string Company { get; set; }

		/// <summary>Gets or sets the custom attributes.</summary>
		[JsonPropertyName("custom_attributes")]
		public CustomAttribute[] CustomAttributes { get; set; }

		/// <summary>Gets or sets the employee's unique ID. .</summary>
		[JsonPropertyName("employee_unique_id")]
		public string EmployeeId { get; set; }

		/// <summary>Gets or sets the group ids.</summary>
		[JsonPropertyName("group_ids")]
		public string[] GroupIds { get; set; }

		/// <summary>Gets or sets the host key.</summary>
		[JsonPropertyName("host_key")]
		public string HostKey { get; set; }

		/// <summary>Gets or sets the IM group ids.</summary>
		[JsonPropertyName("im_group_ids")]
		public string[] ImGroupIds { get; set; }

		/// <summary>Gets or sets the JID.</summary>
		/// <remarks>Zoom's documentation doesn't clarify what a JID is.</remarks>
		[JsonPropertyName("jid")]
		public string JId { get; set; }

		/// <summary>Gets or sets the job title.</summary>
		[JsonPropertyName("job_title")]
		public string JobTitle { get; set; }

		/// <summary>Gets or sets the language for the Zoom Web Portal.</summary>
		[JsonPropertyName("language")]
		public string Language { get; set; }

		/// <summary>Gets or sets the location.</summary>
		[JsonPropertyName("location")]
		public string Location { get; set; }

		/// <summary>Gets or sets the user's login method.</summary>
		[JsonPropertyName("login_type")]
		public LoginType LoginType { get; set; }

		/// <summary>Gets or sets the manager.</summary>
		[JsonPropertyName("manager")]
		public string Manager { get; set; }

		/// <summary>Gets or sets the personal meeting URL.</summary>
		[JsonPropertyName("personal_meeting_url")]
		public string PersonalMeetingUrl { get; set; }

		/// <summary>Gets or sets the country for Company Phone Number.</summary>
		[JsonPropertyName("phone_country")]
		[Obsolete("Replaced by PhoneNumbers")]
		public string PhoneCountry { get; set; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("phone_number")]
		[Obsolete("Replaced by PhoneNumbers")]
		public string PhoneNumber { get; set; }

		/// <summary>Gets or sets the phone numbers.</summary>
		[JsonPropertyName("phone_numbers")]
		public PhoneNumber[] PhoneNumbers { get; set; }

		/// <summary>Gets or sets the URL for the profile picture.</summary>
		[JsonPropertyName("pic_url")]
		public string ProfilePictureUrl { get; set; }

		/// <summary>Gets or sets the united plan type.</summary>
		/// <remarks>Only returned if user is enrolled in the Zoom United plan.</remarks>
		[JsonPropertyName("plan_united_type")]
		public string UnitedPlanType { get; set; }

		/// <summary>Gets or sets the pronouns.</summary>
		[JsonPropertyName("pronouns")]
		public string Pronouns { get; set; }

		/// <summary>Gets or sets the pronouns display setting.</summary>
		[JsonPropertyName("pronouns_option")]
		public PronounDisplayType PronounsDisplay { get; set; }

		/// <summary>Gets or sets the unique identifier of the role assigned to the user.</summary>
		[JsonPropertyName("role_id")]
		public string RoleId { get; set; }

		/// <summary>Gets or sets the status.</summary>
		[JsonPropertyName("status")]
		public UserStatus Status { get; set; }

		/// <summary>Gets or sets the personal meeting room URL, if the user has one.</summary>
		[JsonPropertyName("vanity_url")]
		public string VanityUrl { get; set; }

		/// <summary>Gets or sets a value indicating whether the user is verified or not.</summary>
		[JsonPropertyName("verified")]
		public bool IsVerified { get; set; }
	}
}
