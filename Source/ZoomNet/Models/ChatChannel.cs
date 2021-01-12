using Newtonsoft.Json;

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
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the {Zoom does not provide documentation for this field}.
		/// </summary>
		/// <value>
		/// The JId.
		/// </value>
		[JsonProperty("jid", NullValueHandling = NullValueHandling.Ignore)]
		public string JId { get; set; }

		/// <summary>
		/// Gets or sets the name of the channel.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the channel.
		/// </summary>
		[JsonProperty(PropertyName = "type")]
		public ChatChannelType Type { get; set; }

		/// <summary>
		/// Gets or sets the settings for the channel.
		/// </summary>
		[JsonProperty(PropertyName = "channel_settings")]
		public ChatChannelSettings Settings { get; set; }
	}
}
