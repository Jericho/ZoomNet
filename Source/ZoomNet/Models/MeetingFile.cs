using Newtonsoft.Json;

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
		[JsonProperty("file_name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the URL to download the file.
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		[JsonProperty("download_url", NullValueHandling = NullValueHandling.Ignore)]
		public string DownloadUrl { get; set; }

		/// <summary>
		/// Gets or sets the size of the file (in bytes).
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		[JsonProperty("file_size", NullValueHandling = NullValueHandling.Ignore)]
		public long Size { get; set; }
	}
}
