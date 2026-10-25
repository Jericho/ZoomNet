using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>User.</summary>
	public record User
	{
		/// <summary>Gets or sets the account id.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets the account number.</summary>
		[JsonPropertyName("account_number")]
		public long AccountNumber { get; init; }

		/// <summary>Gets or sets the CMS ID.</summary>
		/// <remarks>Only enabled for Kaltura integration.</remarks>
		[JsonPropertyName("cms_user_id")]
		public string CmsUserId { get; init; }

		/// <summary>Gets or sets the company.</summary>
		[JsonPropertyName("company")]
		public string Company { get; init; }

		/// <summary>Gets or sets the date and time when the user was created.</summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; init; }

		/// <summary>Gets or sets the custom attributes.</summary>
		[JsonPropertyName("custom_attributes")]
		public CustomAttribute[] CustomAttributes { get; init; }

		/// <summary>Gets or sets the department.</summary>
		[JsonPropertyName("dept")]
		public string Department { get; init; }

		/// <summary>Gets or sets a valid email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the employee's unique ID. .</summary>
		[JsonPropertyName("employee_unique_id")]
		public string EmployeeId { get; init; }

		/// <summary>Gets or sets the first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; init; }

		/// <summary>Gets or sets the group ids.</summary>
		[JsonPropertyName("group_ids")]
		public string[] GroupIds { get; init; }

		/// <summary>Gets or sets the host key.</summary>
		[JsonPropertyName("host_key")]
		public string HostKey { get; init; }

		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the IM group ids.</summary>
		[JsonPropertyName("im_group_ids")]
		public string[] ImGroupIds { get; init; }

		/// <summary>Gets or sets the JID.</summary>
		/// <remarks>Zoom's documentation doesn't clarify what a JID is.</remarks>
		[JsonPropertyName("jid")]
		public string JId { get; init; }

		/// <summary>Gets or sets the job title.</summary>
		[JsonPropertyName("job_title")]
		public string JobTitle { get; init; }

		/// <summary>Gets or sets the language for the Zoom Web Portal.</summary>
		[JsonPropertyName("language")]
		public string Language { get; init; }

		/// <summary>Gets or sets the last login client version.</summary>
		[JsonPropertyName("last_client_version")]
		public string LastLoginClientVersion { get; init; }

		/// <summary>Gets or sets the date and time when the user last logged in.</summary>
		[JsonPropertyName("last_login_time")]
		public DateTime LastLogin { get; init; }

		/// <summary>Gets or sets the last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; init; }

		/// <summary>Gets or sets the location.</summary>
		[JsonPropertyName("location")]
		public string Location { get; init; }

		/// <summary>Gets or sets the user's login method.</summary>
		[JsonPropertyName("login_type")]
		public LoginType LoginType { get; init; }

		/// <summary>Gets or sets the manager.</summary>
		[JsonPropertyName("manager")]
		public string Manager { get; init; }

		/// <summary>Gets or sets the personal meeting URL.</summary>
		[JsonPropertyName("personal_meeting_url")]
		public string PersonalMeetingUrl { get; init; }

		/// <summary>Gets or sets the country for Company Phone Number.</summary>
		[JsonPropertyName("phone_country")]
		[Obsolete("Replaced by PhoneNumbers")]
		public string PhoneCountry { get; init; }

		/// <summary>Gets or sets the phone number.</summary>
		[JsonPropertyName("phone_number")]
		[Obsolete("Replaced by PhoneNumbers")]
		public string PhoneNumber { get; init; }

		/// <summary>Gets or sets the phone numbers.</summary>
		[JsonPropertyName("phone_numbers")]
		public PhoneNumber[] PhoneNumbers { get; init; }

		/// <summary>Gets or sets the URL for the profile picture.</summary>
		[JsonPropertyName("pic_url")]
		public string ProfilePictureUrl { get; init; }

		/// <summary>Gets or sets the united plan type.</summary>
		/// <remarks>Only returned if user is enrolled in the Zoom United plan.</remarks>
		[JsonPropertyName("plan_united_type")]
		public string UnitedPlanType { get; init; }

		/// <summary>Gets or sets the personal meeting id.</summary>
		[JsonPropertyName("pmi")]
		public long PersonalMeetingId { get; init; }

		/// <summary>Gets or sets the pronouns.</summary>
		[JsonPropertyName("pronouns")]
		public string Pronouns { get; init; }

		/// <summary>Gets or sets the pronouns display setting.</summary>
		[JsonPropertyName("pronouns_option")]
		public PronounDisplayType PronounsDisplay { get; init; }

		/// <summary>Gets or sets the unique identifier of the role assigned to the user.</summary>
		[JsonPropertyName("role_id")]
		public string RoleId { get; init; }

		/// <summary>Gets or sets the role name.</summary>
		[JsonPropertyName("role_name")]
		public string RoleName { get; init; }

		/// <summary>Gets or sets the status.</summary>
		[JsonPropertyName("status")]
		public UserStatus Status { get; init; }

		/// <summary>Gets or sets the timezone.</summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; init; }

		/// <summary>Gets or sets the type.</summary>
		[JsonPropertyName("type")]
		public UserType Type { get; init; }

		/// <summary>Gets or sets a value indicating whether the PMI is used for instant meetings.</summary>
		[JsonPropertyName("use_pmi")]
		public bool UsePersonalMeetingId { get; init; }

		/// <summary>Gets or sets the personal meeting room URL, if the user has one.</summary>
		[JsonPropertyName("vanity_url")]
		public string VanityUrl { get; init; }

		/// <summary>Gets or sets a value indicating whether the user is verified or not.</summary>
		[JsonPropertyName("verified")]
		public bool IsVerified { get; init; }
	}
}
