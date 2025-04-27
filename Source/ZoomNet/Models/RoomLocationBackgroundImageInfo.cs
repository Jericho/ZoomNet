using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The model of room location basic profile.
	/// </summary>
	public class RoomLocationBackgroundImageInfo
	{
		/// <summary>
		/// Gets or sets the id of the display.
		/// </summary>
		[JsonPropertyName("display_id")]
		public string DisplayId { get; set; }

		/// <summary>
		/// Gets or sets the content id when the background image is a background image library content item or default Zoom Rooms background image library content item.
		/// When a background image has been directly uploaded, this field will be empty.
		/// </summary>
		[JsonPropertyName("content_id")]
		public string ContentId { get; set; }

		/// <summary>
		/// Gets or sets the URL where the background image file may be downloaded.
		/// This URL will expire according to the download_url_.
		/// </summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>
		/// Gets or sets the time to live of the download URL, in seconds.
		/// When this field is not present, the link will never expire.
		/// </summary>
		[JsonPropertyName("download_url_ttl")]
		public int? DownloadUrlTimeToLive { get; set; }
	}
}
