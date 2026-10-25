using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Group.</summary>
	public record Group
	{
		/// <summary>Gets or sets the group id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the group.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the number of member in this group.</summary>
		[JsonPropertyName("total_members")]
		public long NumberOfMembers { get; init; }
	}
}
