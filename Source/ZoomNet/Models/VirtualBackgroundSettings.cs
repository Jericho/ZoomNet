using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Virtual background settings.
	/// </summary>
	public class VirtualBackgroundSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow users to upload custom backgrounds.</summary>
		[JsonPropertyName("allow_upload_custom")]
		public bool AllowCustomBackgrounds { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the use of videos for virtual backgrounds.</summary>
		[JsonPropertyName("allow_videos")]
		public bool AllowVideos { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable virtual backgrounds.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the virtual backgrounds.</summary>
		[JsonPropertyName("files")]
		public VirtualBackgroundFile[] VirtualBackgrounds { get; set; }
	}
}
