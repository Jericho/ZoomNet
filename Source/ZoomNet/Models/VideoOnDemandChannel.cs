using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Video on-demand channel.</summary>
	public record VideoOnDemandChannel
	{
		/// <summary>Gets or sets the channel id.</summary>
		[JsonPropertyName("channel_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the description of the channel.</summary>
		[JsonPropertyName("description")]
		public string Description { get; init; }

		/// <summary>Gets or sets a value indicating whether the content is visible on attendee facing hub profile page.</summary>
		/// <remarks>This field only applies to VIDEO_LIST_HUB type channel.</remarks>
		[JsonPropertyName("is_published_to_hub")]
		public bool IsPublishedToHub { get; init; } = false;

		/// <summary>Gets or sets the name of the channel.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the VOD channel status.</summary>
		[JsonPropertyName("status")]
		public VideoOnDemandChannelStatus Status { get; init; }

		/// <summary>Gets or sets the channel type.</summary>
		[JsonPropertyName("type")]
		public VideoOnDemandChannelType Type { get; init; }
	}
}
