using Newtonsoft.Json;
using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// User.
	/// </summary>
	public class User
	{
		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

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
		/// Gets or sets the type.
		/// </summary>
		[JsonProperty(PropertyName = "type")]
		public UserType Type { get; set; }

		/// <summary>
		/// Gets or sets the personal meeting id.
		/// </summary>
		[JsonProperty(PropertyName = "pmi")]
		public string PersonalMeetingId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the PMI is used for instant meetings.
		/// </summary>
		[JsonProperty(PropertyName = "use_pmi")]
		public bool UsePersonalMeetingId { get; set; }

		/// <summary>
		/// Gets or sets the timezone.
		/// </summary>
		[JsonProperty(PropertyName = "timezone")]
		public string Timezone { get; set; }

		/// <summary>
		/// Gets or sets the department.
		/// </summary>
		[JsonProperty(PropertyName = "dept")]
		public string Department { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the user was created.
		/// </summary>
		/// <value>The user created time.</value>
		[JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the user last logged in.
		/// </summary>
		/// <value>The login time.</value>
		[JsonProperty(PropertyName = "last_login_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime LastLogin { get; set; }

		/// <summary>
		/// Gets or sets the last login client version.
		/// </summary>
		[JsonProperty(PropertyName = "last_client_version")]
		public string LastLoginClientVersion { get; set; }

		/// <summary>
		/// Gets or sets the language for the Zoom Web Portal.
		/// </summary>
		[JsonProperty(PropertyName = "language")]
		public string Language { get; set; }

		/// <summary>
		/// Gets or sets the country for Company Phone Number.
		/// </summary>
		[JsonProperty(PropertyName = "phone_country")]
		public string PhoneCountry { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		[JsonProperty(PropertyName = "phone_number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the personal meeting room URL, if the user has one.
		/// </summary>
		[JsonProperty(PropertyName = "vanity_url")]
		public string VanityUrl { get; set; }

		/// <summary>
		/// Gets or sets the personal meeting URL.
		/// </summary>
		[JsonProperty(PropertyName = "personal_meeting_url")]
		public string PersonalMeetingUrl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is verified or not.
		/// </summary>
		[JsonProperty(PropertyName = "verified")]
		public bool IsVerified { get; set; }

		/// <summary>
		/// Gets or sets the URL for the profile picture.
		/// </summary>
		[JsonProperty(PropertyName = "pic_url")]
		public string ProfilePictureUrl { get; set; }

		/// <summary>
		/// Gets or sets the CMS ID.
		/// </summary>
		/// <remarks>
		/// Only enabled for Kaltura integration.
		/// </remarks>
		[JsonProperty(PropertyName = "cms_user_id")]
		public string CmsUserId { get; set; }

		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		[JsonProperty(PropertyName = "account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the host key.
		/// </summary>
		[JsonProperty(PropertyName = "host_key")]
		public string HostKey { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		[JsonProperty(PropertyName = "status")]
		public UserStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the group ids.
		/// </summary>
		[JsonProperty(PropertyName = "group_ids")]
		public string[] GroupIds { get; set; }

		/// <summary>
		/// Gets or sets the IM group ids.
		/// </summary>
		[JsonProperty(PropertyName = "im_group_ids")]
		public string[] ImGroupIds { get; set; }

		/// <summary>
		/// Gets or sets the ???.
		/// </summary>
		[JsonProperty(PropertyName = "jid")]
		public string JId { get; set; }

		/// <summary>
		/// Gets or sets the job title.
		/// </summary>
		[JsonProperty(PropertyName = "job_title")]
		public string JobTitle { get; set; }

		/// <summary>
		/// Gets or sets the company.
		/// </summary>
		[JsonProperty(PropertyName = "company")]
		public string Company { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		[JsonProperty(PropertyName = "location")]
		public string Location { get; set; }
	}
}
