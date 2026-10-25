using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Member in a channel.</summary>
	public record ChatChannelMember
	{
		/// <summary>Gets or sets a valid email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the first name.</summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; init; }

		/// <summary>Gets or sets the member id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the last name.</summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; init; }

		/// <summary>Gets or sets the role.</summary>
		[JsonPropertyName("role")]
		public ChatChannelRole Role { get; init; }
	}
}
