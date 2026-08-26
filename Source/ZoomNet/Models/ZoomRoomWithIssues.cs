using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Zoom room with issues.</summary>
	public class ZoomRoomWithIssues
	{
		/// <summary>Gets or sets the Zoom room id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the count of issues in the Zoom room.</summary>
		[JsonPropertyName("issues_count")]
		public int IssuesCount { get; set; }

		/// <summary>Gets or sets the Zoom room name.</summary>
		[JsonPropertyName("room_name")]
		public string Name { get; set; }
	}
}
