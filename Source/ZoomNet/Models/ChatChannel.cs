using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat channel.
	/// </summary>
	public class ChatChannel
	{
		/// <summary>
		/// Gets or sets the channel id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the {Zoom does not provide documentation for this field}.
		/// </summary>
		/// <value>
		/// The JId.
		/// </value>
		[JsonPropertyName("jid")]
		public string JId { get; set; }

		/// <summary>
		/// Gets or sets the name of the channel.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the channel.
		/// </summary>
		[JsonPropertyName("type")]
		public ChatChannelType Type { get; set; }

		/// <summary>
		/// Gets or sets the settings for the channel.
		/// </summary>
		[JsonPropertyName("channel_settings")]
		public ChatChannelSettings Settings { get; set; }
	}
}
