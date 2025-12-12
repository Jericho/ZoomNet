using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that control ad-hoc call recording.
	/// </summary>
	public class AdHocCallRecordingSettings : CallRecordingSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow user to delete their own ad-hoc recording.
		/// </summary>
		[JsonPropertyName("allow_delete")]
		public bool? AllowDelete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow user to download their own ad-hoc recording.
		/// </summary>
		[JsonPropertyName("allow_download")]
		public bool? AllowDownload { get; set; }
	}
}
