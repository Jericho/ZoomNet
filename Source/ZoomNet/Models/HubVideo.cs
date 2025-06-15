using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A video/recording of a hub.
	/// </summary>
	public class HubVideo
	{
		/// <summary>Gets or sets the video id.</summary>
		[JsonPropertyName("video_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the video source type.</summary>
		[JsonPropertyName("source_type")]
		public HubVideoSourceType SourceType { get; set; }

		/// <summary>Gets or sets the ID of the Zoom Event this video belongs to.</summary>
		/// <remarks>If this is not associated to any Zoom Event then it is blank.</remarks>
		[JsonPropertyName("source_event_id")]
		public string SourceEventId { get; set; }

		/// <summary>Gets or sets the status.</summary>
		[JsonPropertyName("status")]
		public HubVideoStatus Status { get; set; }

		/// <summary>Gets or sets the play URL.</summary>
		[JsonPropertyName("play_link")]
		public string PlayUrl { get; set; }

		/// <summary>Gets or sets the video duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }
	}
}
