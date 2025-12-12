using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that control voicemail and videomail access and behavior.
	/// </summary>
	public class VoicemailSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether users are allowed to delete their own voicemail or videomail.
		/// </summary>
		[JsonPropertyName("allow_delete")]
		public bool? AllowDelete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether users are allowed to download their own voicemail or videomail.
		/// </summary>
		[JsonPropertyName("allow_download")]
		public bool? AllowDownload { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether users are allowed to share their own voicemail.
		/// </summary>
		[JsonPropertyName("allow_share")]
		public bool? AllowShare { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether videomail is allowed.
		/// </summary>
		[JsonPropertyName("allow_videomail")]
		public bool? AllowVideomail { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether virtual background for voicemail or videomail is allowed.
		/// </summary>
		[JsonPropertyName("allow_virtual_background")]
		public bool? AllowVirtualBackground { get; set; }
	}
}
