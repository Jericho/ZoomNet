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
		/// Gets or sets the group ids.
		/// </summary>
		[JsonProperty(PropertyName = "group_ids")]
		public string[] GroupIds { get; set; }

		/// <summary>
		/// Gets or sets the IM group ids.
		/// </summary>
		[JsonProperty(PropertyName = "im_group_ids")]
		public string[] ImGroupIds { get; set; }
	}
}
