using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A file sent via in-meeting chat during a meeting.</summary>
	public class MeetingFile
	{
		/// <summary>Gets or sets the URL to download the file.</summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the name of the file.</summary>
		[JsonPropertyName("file_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the size of the file (in bytes).</summary>
		[JsonPropertyName("file_size")]
		public long Size { get; set; }
	}
}
