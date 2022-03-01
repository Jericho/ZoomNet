using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Virtual background settings.
	/// </summary>
	public class VirtualBackgroundSettings
	{
		/// <summary>Gets or sets a value indicating whether to allow users to upload custom backgrounds.</summary>
		[JsonProperty(PropertyName = "allow_upload_custom")]
		public bool AllowCustomBackgrounds { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow the use of videos for virtual backgrounds.</summary>
		[JsonProperty(PropertyName = "allow_videos")]
		public bool AllowVideos { get; set; }

		/// <summary>Gets or sets a value indicating whether to enable virtual backgrounds.</summary>
		[JsonProperty(PropertyName = "enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the virtual backgrounds.</summary>
		[JsonProperty(PropertyName = "files")]
		public VirtualBackgroundFile[] VirtualBackgrounds { get; set; }
	}
}
