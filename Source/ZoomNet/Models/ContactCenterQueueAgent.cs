using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An agent assigned to a given Contect Center queue.
	/// </summary>
	public class ContactCenterQueueAgent
	{
		/// <summary>Gets or sets the agent's name.</summary>
		[JsonPropertyName("display_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the Opt in / Opt out status of the agent.</summary>
		[JsonPropertyName("opt_in_out_status")]
		public ContactCenterQueueOptinStatus OptInStatus { get; set; }

		/// <summary>Gets or sets the role ID.</summary>
		[JsonPropertyName("role_id")]
		public string RoleId { get; set; }

		/// <summary>Gets or sets the name of the role.</summary>
		[JsonPropertyName("role_name")]
		public string RoleName { get; set; }

		/// <summary>Gets or sets the agent's status ID.</summary>
		[JsonPropertyName("status_id")]
		public string StatusId { get; set; }

		/// <summary>Gets or sets the agent's availability status.</summary>
		[JsonPropertyName("status_name")]
		public string StatusName { get; set; }

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
