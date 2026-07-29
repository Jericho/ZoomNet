using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents an item in a Zoom Rooms Digital Signage content playlist.</summary>
	public class DigitalSignageContentPlaylistItem
	{
		/// <summary>Gets or sets the content item ID.</summary>
		[JsonPropertyName("content_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the content item.</summary>
		[JsonPropertyName("content_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the duration of the content item in seconds.</summary>
		[JsonPropertyName("content_duration")]
		public int Duration { get; set; }
	}
}
