using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "share_recording")]
		public RecordingSharingType SharingType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets the value indicating whether only authenticated user can view the recording.
		/// </summary>
		[JsonProperty(PropertyName = "recording_authentication")]
		public bool RequiresAuthentication { get; set; }

		/// <summary>
		/// Gets or sets the authentication options.
		/// </summary>
		[JsonProperty(PropertyName = "authentication_option")]
		public string AuthenticationOptions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the recording can be downloaded.
		/// </summary>
		[JsonProperty(PropertyName = "viewer_download")]
		public bool AllowDownload { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		[JsonProperty(PropertyName = "password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether registration is required to view the recording.
		/// </summary>
		[JsonProperty(PropertyName = "on_demand")]
		public bool RegistrationRequired { get; set; }

		/// <summary>
		/// Gets or sets the approval type for the registration.
		/// </summary>
		[JsonProperty(PropertyName = "approval_type")]
		public MeetingApprovalType RegistrationApprovalType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to send an email to the host when someone registers to view the recording.
		/// </summary>
		[JsonProperty(PropertyName = "send_email_to_host")]
		public bool NotifyHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show social share buttons on registration page.
		/// </summary>
		[JsonProperty(PropertyName = "show_social_share_buttons")]
		public bool ShowSocialShareButtons { get; set; }
	}
}
