using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// MEmeber of a channel mention group.
	/// </summary>
	public class ChannelMentionGroupMember
	{
		/// <summary>Gets or sets the user id.</summary>
		[JsonPropertyName("user_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the member id.</summary>
		[JsonPropertyName("member_id")]
		public string MemberId { get; set; }

		/// <summary>Gets or sets a valid email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>Gets or sets the last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>Gets or sets the display name.</summary>
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		/// <summary>Gets or sets a value indicating whether it is an external member.</summary>
		[JsonPropertyName("is_external")]
		public bool IsExternal { get; set; }
	}
}
