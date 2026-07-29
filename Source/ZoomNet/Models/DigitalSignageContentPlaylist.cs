using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a playlist in the Zoom Rooms Digital Signage content library.</summary>
	public class DigitalSignageContentPlaylist
	{
		/// <summary>Gets or sets the playlist ID.</summary>
		[JsonPropertyName("playlist_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the playlist.</summary>
		[JsonPropertyName("playlist_name")]
		public string Name { get; set; }
	}
}
