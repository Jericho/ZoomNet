using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents a channel mention group model.
	/// </summary>
	public class ChannelMentionGroup
	{
		/// <summary>Gets or sets the mention group ID.</summary>
		[JsonPropertyName("mention_group_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the mention group's name.</summary>
		[JsonPropertyName("mention_group_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the mention group's description.</summary>
		[JsonPropertyName("mention_group_description")]
		public string Description { get; set; }
	}
}
