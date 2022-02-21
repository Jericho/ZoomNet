using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A file sent via in-meeting chat during a meeting.
	/// </summary>
	public class MeetingFile
	{
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[JsonPropertyName("file_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the URL to download the file.
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>
		/// Gets or sets the size of the file (in bytes).
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		[JsonPropertyName("file_size")]
		public long Size { get; set; }
	}
}
