using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// 'Mention' in a chat message.
	/// </summary>
	public class ChatMention
	{
		/// <summary>
		/// Gets or sets the start position of the mention(\"@\") in the message string.
		/// For example if you want to include the mention at the beginning of the message,
		/// the value for this field will be 0.
		/// </summary>
		[JsonPropertyName("start_position")]
		public int Start { get; set; }

		/// <summary>
		/// Gets or sets the end position of the mention.
		/// Example message: \"@Shrijana How are you?\".
		/// In this case, the end position of the mention \"@Shrijana\" is 8.
		/// </summary>
		[JsonPropertyName("end_position")]
		public int End { get; set; }

		/// <summary>
		/// Gets or sets the type of mention.
		/// </summary>
		[JsonPropertyName("at_type")]
		public ChatMentionType Type { get; set; }

		/// <summary>
		/// Gets or sets the contact being mentioned.
		/// </summary>
		/// <remarks>Will be empty if <see cref="Type"/> is <see cref="ChatMentionType.All"/>.</remarks>
		[JsonPropertyName("at_type")]
		public string Contact { get; set; }
	}
}
