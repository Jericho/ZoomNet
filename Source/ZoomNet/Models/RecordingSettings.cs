using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting recording settings.
	/// </summary>
	public class RecordingSettings
	{
		/// <summary>
		/// Gets or sets the value indicating how the meeting recording is shared.
		/// </summary>
		[JsonPropertyName("share_recording")]
		public RecordingSharingType SharingType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets the value indicating whether only authenticated user can view the recording.
		/// </summary>
		[JsonPropertyName("recording_authentication")]
		public bool RequiresAuthentication { get; set; }

		/// <summary>
		/// Gets or sets the authentication options.
		/// </summary>
		[JsonPropertyName("authentication_option")]
		public string AuthenticationOptions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the recording can be downloaded.
		/// </summary>
		[JsonPropertyName("viewer_download")]
		public bool AllowDownload { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		[JsonPropertyName("password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether registration is required to view the recording.
		/// </summary>
		[JsonPropertyName("on_demand")]
		public bool RegistrationRequired { get; set; }

		/// <summary>
		/// Gets or sets the approval type for the registration.
		/// </summary>
		[JsonPropertyName("approval_type")]
		public ApprovalType RegistrationApprovalType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an email to the host when someone registers to view the recording.
		/// </summary>
		[JsonPropertyName("send_email_to_host")]
		public bool NotifyHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show social share buttons on registration page.
		/// </summary>
		[JsonPropertyName("show_social_share_buttons")]
		public bool ShowSocialShareButtons { get; set; }
	}
}
