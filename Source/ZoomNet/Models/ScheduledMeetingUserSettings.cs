using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Scheduled Meeting user settings.
	/// </summary>
	public class ScheduledMeetingUserSettings
	{
		/// <summary>Gets or sets a value indicating how participants can join the audio portion of the meeting.</summary>
		[JsonPropertyName("audio_type")]
		public AudioType Audio { get; set; }

		/// <summary>Gets or sets the default password for scheduled meetings.</summary>
		[JsonPropertyName("default_password_for_scheduled_meetings")]
		public string DefaultPasswordForSchedulingMeetings { get; set; }

		/// <summary>Gets or sets a value indicating whether the meeting password will be encrypted and included in the join meeting link.</summary>
		[JsonPropertyName("embed_password_in_join_link")]
		public bool EmbedPasswordInJoinLink { get; set; }

		/// <summary>Gets or sets a value indicating whether a password is required if attendees can join before host.</summary>
		[JsonPropertyName("force_pmi_jbh_password")]
		public bool PasswordRequiredToJoinBeforeHost { get; set; }

		/// <summary>Gets or sets a value indicating whether to start video when host joins the meeting.</summary>
		[JsonPropertyName("host_video")]
		public bool StartVideoWhenHostJoins { get; set; }

		/// <summary>Gets or sets a value indicating whether participants can join the meeting before the host starts the meeting. Only used for scheduled or recurring meetings.</summary>
		[JsonPropertyName("join_before_host")]
		public bool JoinBeforeHost { get; set; }

		/// <summary>Gets or sets the password requirements.</summary>
		[JsonPropertyName("meeting_password_requirement")]
		public PasswordRequirements PasswordRequirements { get; set; }

		/// <summary>Gets or sets a value indicating whether to start video when participants join the meeting.</summary>
		[JsonPropertyName("participant_video")]
		public bool StartVideoWhenParticipantsJoin { get; set; }

		/// <summary>Gets or sets a value indicating whether the meeting password will be encrypted and included in the join meeting link.</summary>
		[JsonPropertyName("personal_meeting")]
		public bool EnablePersonalMeetingId { get; set; }

		/// <summary>Gets or sets the PMI password.</summary>
		[JsonPropertyName("pmi_password")]
		public string PmiPassword { get; set; }

		/// <summary>Gets or sets a value indicating whether a password is required for participants joining by phone.</summary>
		[JsonPropertyName("pstn_password_protected")]
		public bool PasswordRequiredToJoinByPhone { get; set; }

		/// <summary>Gets or sets a value indicating whether a password is required for instant meetings.</summary>
		[JsonPropertyName("require_password_for_instant_meetings")]
		public bool RequirePasswordForInstantMeetings { get; set; }

		/// <summary>Gets or sets a value indicating whether a password is required for meeting that have already been scheduled.</summary>
		[JsonPropertyName("require_password_for_pmi_meetings")]
		public PmiMeetingPasswordRequirementType RequirePasswordForPmiMeetings { get; set; }

		/// <summary>Gets or sets a value indicating whether a password is required for meeting that have already been scheduled. </summary>
		[JsonPropertyName("require_password_for_scheduled_meetings")]
		public bool RequirePasswordForScheduledMeetings { get; set; }

		/// <summary>Gets or sets a value indicating whether a password is required when scheduling a new meeting.</summary>
		[JsonPropertyName("require_password_for_scheduling_new_meetings")]
		public bool RequirePasswordForSchedulingNewMeetings { get; set; }

		/// <summary>Gets or sets a value indicating whether to use Personal Meeting ID when starting an instant meeting.</summary>
		[JsonPropertyName("use_pmi_for_instant_meetings")]
		public bool UsePmiForInstantMeetings { get; set; }

		/// <summary>Gets or sets a value indicating whether to use Personal Meeting ID when scheduling a meeting.</summary>
		[JsonPropertyName("use_pmi_for_scheduled_meetings")]
		public bool UsePmiForScheduledMeetings { get; set; }
	}
}
