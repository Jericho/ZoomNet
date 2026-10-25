using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Role.</summary>
	public record Role
	{
		/// <summary>Gets or sets the description.</summary>
		[JsonPropertyName("description")]
		public string Description { get; init; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the role.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the number of members in this role.</summary>
		[JsonPropertyName("total_members")]
		public long MembersCount { get; init; }
	}
}
