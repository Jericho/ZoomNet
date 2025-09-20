using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Contact Center user role.
	/// </summary>
	public class ContactCenterUserRole
	{
		/// <summary>Gets or sets the date and time at which the role was last modified.</summary>
		[JsonPropertyName("last_modified_time")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>Gets or sets the ID of the user who last modified the role.</summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

		/// <summary>Gets or sets the description.</summary>
		[JsonPropertyName("role_description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("role_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the role.</summary>
		[JsonPropertyName("role_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the number of users in this role.</summary>
		[JsonPropertyName("total_users")]
		public long UsersCount { get; set; }
	}
}
