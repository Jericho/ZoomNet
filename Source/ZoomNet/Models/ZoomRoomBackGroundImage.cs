using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a Zoom Room background image.</summary>
	public class ZoomRoomBackGroundImage
	{
		/// <summary>Gets or sets the id of the image.</summary>
		[JsonPropertyName("content_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the image.</summary>
		[JsonPropertyName("content_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the download URL of the image.</summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the time-to-live (TTL) for the download URL.</summary>
		[JsonPropertyName("download_url_ttl")]
		public int DownloadUrlTimeToLive { get; set; }
	}
}
