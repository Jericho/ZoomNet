using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a playlist in the Zoom Rooms Digital Signage content library.</summary>
	public record DigitalSignageContentPlaylist
	{
		/// <summary>Gets or sets the playlist ID.</summary>
		[JsonPropertyName("playlist_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the playlist.</summary>
		[JsonPropertyName("playlist_name")]
		public string Name { get; init; }
	}
}
