using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A user of the Contect Center.
	/// </summary>
	public class ContactCenterUser
	{
		/// <summary>Gets or sets the Zoom Contact Center Plans to assign to the user.</summary>
		[JsonPropertyName("add_ons_plan")]
		public string[] AddonPlans { get; set; }

		/// <summary>Gets or sets the information about the channel settings for the contact center user.</summary>
		[JsonPropertyName("channel_settings")]
		public ContactCenterUserChannelSettings ChannelSettings { get; set; }

		/// <summary>Gets or sets the contact center's client integration.</summary>
		[JsonPropertyName("client_integration")]
		public ContactCenterClientIntegration ClientIntegration { get; set; }

		/// <summary>Gets or sets the two-letter country code.</summary>
		[JsonPropertyName("country_iso_code")]
		public string CountryIsoCode { get; set; }

		/// <summary>Gets or sets the user's name.</summary>
		[JsonPropertyName("display_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the date and time when the user was last modified.</summary>
		[JsonPropertyName("last_modified_time")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>Gets or sets the ID of the user that last modified the user.</summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

		/// <summary>Gets or sets the ID of the user that last modified the user.</summary>
		[JsonPropertyName("package_name")]
		//should be enum
		public string PackageName { get; set; }

		/// <summary>Gets or sets the user's region ID.</summary>
		[JsonPropertyName("region_id")]
		public string RegionId { get; set; }

		/// <summary>Gets or sets the user's region name.</summary>
		[JsonPropertyName("region_name")]
		public string RegionName { get; set; }

		/// <summary>Gets or sets the user's status ID.</summary>
		[JsonPropertyName("status_id")]
		public string StatusId { get; set; }

		/// <summary>Gets or sets the user's availability status.</summary>
		[JsonPropertyName("status_name")]
		public string StatusName { get; set; }

		/// <summary>Gets or sets the user's reason ID when the status is 'Not Ready'.</summary>
		[JsonPropertyName("sub_status_id")]
		public string SubStatusId { get; set; }

		/// <summary>Gets or sets the user's reason when the status is 'Not Ready'.</summary>
		[JsonPropertyName("sub_status_name")]
		public string SubStatusName { get; set; }

		/// <summary>Gets or sets the user's access status.</summary>
		[JsonPropertyName("user_access")]
		//should be enum
		public string Status { get; set; }

		/// <summary>Gets or sets the user's email address.</summary>
		[JsonPropertyName("user_email")]
		public string EmailAddress { get; set; }

		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("user_id")]
		public string Id { get; set; }
	}
}
