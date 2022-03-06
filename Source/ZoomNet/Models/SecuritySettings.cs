using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Security settings.
	/// </summary>
	public class SecuritySettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether all meetings must be secured with at least one security options.
		/// </summary>
		[JsonPropertyName("auto_security")]
		public bool MustBeSecured { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether users in specific domains are blocked from joining meetings and webinars.
		/// </summary>
		[JsonPropertyName("block_user_domain")]
		public bool EnforceBlockedDomains { get; set; }

		/// <summary>
		/// Gets or sets the blocked domains.
		/// </summary>
		[JsonPropertyName("block_user_domain_list")]
		public string[] BlockedDomains { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meeting password is encrypted and included in the invitation link.
		/// </summary>
		[JsonPropertyName("embed_password_in_join_link")]
		public bool IncludePasswordInJoinLink { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meeting password is encrypted and included in the invitation link.
		/// </summary>
		[JsonPropertyName("encryption_type")]
		public EncryptionType EncryptionType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether end-to-end encryption is enabled.
		/// </summary>
		[JsonPropertyName("end_to_end_encrypted_meetings")]
		public bool EndToEndEncryptionEnabled { get; set; }

		/// <summary>
		/// Gets or sets the password requirements.
		/// </summary>
		[JsonPropertyName("meeting_password_requirement")]
		public PasswordRequirements PasswordRequirements { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether only authenticated users can join a meeting from the web client.
		/// </summary>
		[JsonPropertyName("only_authenticated_can_join_from_webclient")]
		public bool OnlyAuthenticatedCanJoinFromWebclient { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether passwords are required for participants joining by phone.
		/// </summary>
		[JsonPropertyName("phone_password")]
		public bool PasswordIsRequiredToJoinPhone { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether passwords are required for participants joining a Personal Meeting ID meeting.
		/// </summary>
		[JsonPropertyName("pmi_password")]
		public bool PasswordIsRequiredToJoinPMI { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether passwords are required for participants joining a scheduled meeting.
		/// </summary>
		[JsonPropertyName("require_password_for_scheduled_meeting")]
		public bool PasswordIsRequiredToJoinScheduledMeeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether passwords are required for participants joining a scheduled webinar.
		/// </summary>
		[JsonPropertyName("require_password_for_scheduled_webinar")]
		public bool PasswordIsRequiredToJoinScheduledWebinar { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether participants are placed in the Waiting Room when they join a meeting.
		/// </summary>
		[JsonPropertyName("waiting_room")]
		public bool JoinWaitingRoomBeforeMeeting { get; set; }

		/// <summary>
		/// Gets or sets the waiting room settings.
		/// </summary>
		[JsonPropertyName("waiting_room_settings")]
		public WaitingRoomSettings WaitingRoomSettings { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a password is generated when scheduling webinars.
		/// Participants must use the generated password to join the scheduled webinar.
		/// </summary>
		[JsonPropertyName("webinar_password")]
		public bool GeneratePasswordForScheduledWebinars { get; set; }
	}
}
