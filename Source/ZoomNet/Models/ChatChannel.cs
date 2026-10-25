using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Chat channel.</summary>
	public record ChatChannel
	{
		/// <summary>Gets or sets the channel id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the {Zoom does not provide documentation for this field}.</summary>
		[JsonPropertyName("jid")]
		public string JId { get; init; }

		/// <summary>Gets or sets the name of the channel.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the type of the channel.</summary>
		[JsonPropertyName("type")]
		public ChatChannelType Type { get; init; }

		/// <summary>Gets or sets the settings for the channel.</summary>
		[JsonPropertyName("channel_settings")]
		public ChatChannelSettings Settings { get; init; }
	}
}
