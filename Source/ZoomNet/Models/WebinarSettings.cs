using System;
using System.Text.Json.Serialization;
using ZoomNet.Resources;

namespace ZoomNet.Models
{
	/// <summary>Webinar Settings.</summary>
	public class WebinarSettings
	{
		/// <summary>Gets or sets the value indicating whether to add an audio watermark when recording.</summary>
		[JsonPropertyName("add_audio_watermark")]
		public bool? AddAudioWatermark { get; set; }

		/// <summary>Gets or sets the value indicating whether to add a watermark when viewing a shared screen.</summary>
		[JsonPropertyName("add_watermark")]
		public bool? AddWatermark { get; set; }

		/// <summary>Gets or sets the list of additional data center regions for hosting the webinar.</summary>
		[JsonPropertyName("additional_data_center_regions")]
		public string[] AdditionalDataCenterRegions { get; set; }

		/// <summary>Gets or sets the value indicating whether to allow the host to control participant mute state.</summary>
		[JsonPropertyName("allow_host_control_participant_mute_state")]
		public bool? AllowHostToControlParticipantMuteState { get; set; }

		/// <summary>Gets or sets the value indicating whether to allow attendees to join from multiple devices.</summary>
		[JsonPropertyName("allow_multiple_devices")]
		public bool? AllowMultipleDevices { get; set; }

		/// <summary>Gets or sets the value indicating whether to allow alternative hosts to add or edit polls.</summary>
		[JsonPropertyName("alternative_host_update_polls")]
		public bool? AllowAlternativeHostToUpdatePolls { get; set; }

		/// <summary>Gets or sets the value indicating alternative hosts emails or IDs. Multiple value separated by comma.</summary>
		[JsonPropertyName("alternative_hosts")]
		public string AlternativeHosts { get; set; }

		/// <summary>Gets or sets the approval type.</summary>
		[JsonPropertyName("approval_type")]
		public ApprovalType? ApprovalType { get; set; }

		/// <summary>Gets or sets the value indicating whether to send reminder emails to attendees and panelists and, if so, when the reminder should be sent.</summary>
		[JsonPropertyName("attendees_and_panelists_reminder_email_notification")]
		public ReminderSettings ReminderSettings { get; set; }

		/// <summary>Gets or sets the value indicating how participants can join the audio portion of the webinar.</summary>
		[JsonPropertyName("audio")]
		public AudioType? Audio { get; set; }

		/// <summary>Gets or sets the third party audio conference info.</summary>
		[JsonPropertyName("audio_conference_info")]
		public string ThirdPartyAudioConferenceInfo { get; set; }

		/// <summary>Gets or sets the list of domains that are authenticated if user has configured "Sign Into Zoom with Specified Domains" option.</summary>
		[JsonPropertyName("authentication_domains")]
		public string AuthenticationDomains { get; set; }

		/// <summary>Gets or sets the authentication name set in the authentication profile.</summary>
		[JsonPropertyName("authentication_name")]
		public string AuthenticationName { get; set; }

		/// <summary>Gets or sets the authentication type for users to join a webinar when <see cref="AuthenticatedUsersOnly"/> is set to true.</summary>
		/// <remarks>The value of this field can be retrieved from the <see cref="AuthenticationOptions.Id"/> in the response of <see cref="IUsers.GetMeetingAuthenticationSettingsAsync"/>.</remarks>
		[JsonPropertyName("authentication_option")]
		public string AuthenticationTypeId { get; set; }

		/// <summary>Gets or sets the value indicating if audio is recorded and if so, where the audio is saved.</summary>
		[JsonPropertyName("auto_recording")]
		public AutoRecordingType AutoRecording { get; set; }

		/// <summary>Gets or sets the value indicating whether registration is closed after event date.</summary>
		[JsonPropertyName("close_registration")]
		public bool? CloseRegistration { get; set; }

		/// <summary>Gets or sets the contact email for registration.</summary>
		[JsonPropertyName("contact_email")]
		public string ContactEmail { get; set; }

		/// <summary>Gets or sets the contact name for registration.</summary>
		[JsonPropertyName("contact_name")]
		public string ContactName { get; set; }

		/// <summary>Gets or sets the value indicating whether to include email addresses in the attendee report.</summary>
		[JsonPropertyName("email_in_attendee_report")]
		public bool? IncludeEmailInAttendeeReport { get; set; }

		/// <summary>Gets or sets the language for emails sent to panelists and registrants.</summary>
		[JsonPropertyName("email_language")]
		public Language EmailLanguage { get; set; }

		/// <summary>Gets or sets the value indicating whether to enable session branding.</summary>
		[JsonPropertyName("enable_session_branding")]
		public bool? EnableSessionBranding { get; set; }

		/// <summary>Gets or sets the value indicating that only signed-in users can join this webinar.</summary>
		[JsonPropertyName("enforce_login")]
		[Obsolete("This field is deprecated and will not be supported in the future. Use AuthenticatedUsersOnly, AuthenticationTypeId and AuthenticationDomains to understand the authentication configurations set for the webinar.")]
		public bool? EnforceLogin { get; set; }

		/// <summary>Gets or sets the value indicating only signed-in users with specified domains can join this webinar.</summary>
		[JsonPropertyName("enforce_login_domains")]
		[Obsolete("This field is deprecated and will not be supported in the future. Use AuthenticatedUsersOnly, AuthenticationTypeId and AuthenticationDomains to understand the authentication configurations set for the webinar.")]
		public string EnforceLoginDomains { get; set; }

		/// <summary>Gets or sets the value indicating only signed-in users with specified domains can join this webinar.</summary>
		[JsonPropertyName("follow_up_absentees_email_notification")]
		public FollowupEmailNotificationSettings FollowUpAbsenteesEmailNotification { get; set; }

		/// <summary>Gets or sets the value indicating only signed-in users with specified domains can join this webinar.</summary>
		[JsonPropertyName("follow_up_attendees_email_notification")]
		public FollowupEmailNotificationSettings FollowUpAttendeesEmailNotification { get; set; }

		/// <summary>Gets or sets the list of global dial-in countries.</summary>
		[JsonPropertyName("global_dial_in_countries")]
		public DialInInfo[] DialInInfo { get; set; } = Array.Empty<DialInInfo>();

		/// <summary>Gets or sets the value indicating whether to enable HD video.</summary>
		[JsonPropertyName("hd_video")]
		public bool? EnableHighDefinitionVideo { get; set; }

		/// <summary>Gets or sets the value indicating whether to enable HD video for attendees.</summary>
		[JsonPropertyName("hd_video_for_attendees")]
		public bool? EnableHighDefinitionVideoForAttendees { get; set; }

		/// <summary>Gets or sets the value indicating whether to start video when host joins the webinar.</summary>
		[JsonPropertyName("host_video")]
		public bool? StartVideoWhenHostJoins { get; set; }

		/// <summary>Gets or sets the language interpretation settings.</summary>
		[JsonPropertyName("language_interpretation")]
		public LanguageInterpretationSettings LanguageInterpretationSettings { get; set; }

		/// <summary>Gets or sets the value indicating that only authenticated users can join the webinar.</summary>
		[JsonPropertyName("meeting_authentication")]
		public bool? AuthenticatedUsersOnly { get; set; }

		/// <summary>Gets or sets the value indicating whether to make the webinar on-demand.</summary>
		[JsonPropertyName("on_demand")]
		public bool? OnDemand { get; set; }

		/// <summary>Gets or sets the value indicating whether to require authentication for panelists.</summary>
		[JsonPropertyName("panelist_authentication")]
		public bool? PanelistAuthentication { get; set; }

		/// <summary>Gets or sets the value indicating whether to send invitation email to panelists.</summary>
		[JsonPropertyName("panelists_invitation_email_notification")]
		public bool? SendInvitationEmailToPanelists { get; set; }

		/// <summary>Gets or sets the value indicating whether to start video when panelists join the webinar.</summary>
		[JsonPropertyName("panelists_video")]
		public bool? StartVideoWhenPanelistsJoin { get; set; }

		/// <summary>Gets or sets the value indicating whether to enable post-webinar survey.</summary>
		[JsonPropertyName("post_webinar_survey")]
		public bool? EnablePostWebinarSurvey { get; set; }

		/// <summary>Gets or sets the value indicating whether to enable practice session.</summary>
		[JsonPropertyName("practice_session")]
		public bool? EnablePracticeSession { get; set; }

		/// <summary>Gets or sets the value indicating whether to send confirmation email to registrants.</summary>
		[JsonPropertyName("registrants_confirmation_email")]
		public bool? SendConfirmationEmailToRegistrants { get; set; }

		/// <summary>Gets or sets the value indicating whether email notifications are sent about approval, cancellation, denial of registration.</summary>
		[JsonPropertyName("registrants_email_notification")]
		public bool? SendRegistrationConfirmationEmail { get; set; }

		/// <summary>Gets or sets the maximum number of registrants.</summary>
		/// <remarks>Omitting this value, setting it to a negative value or setting it to zero indicates that the number of registrants will not be restricted.</remarks>
		[JsonPropertyName("registrants_restrict_number")]
		public int? MaximumNumberOfRegistrants { get; set; }

		/// <summary>Gets or sets the registration type. Used for recurring webinar with fixed time only.</summary>
		[JsonPropertyName("registration_type")]
		public RegistrationType? RegistrationType { get; set; }

		/// <summary>Gets or sets the value indicating whether to request permission to unmute participants.</summary>
		[JsonPropertyName("request_permission_to_unmute_participants")]
		public bool? RequestPermissionToUnmuteParticipants { get; set; }

		/// <summary>Gets or sets the value indicating whether to send 1080p video to attendees.</summary>
		[JsonPropertyName("send_1080p_video_to_attendees")]
		public bool? Send1080pVideoToAttendees { get; set; }

		/// <summary>Gets or sets the value indicating whether to show join info on the registration page.</summary>
		[JsonPropertyName("show_join_info")]
		public bool? ShowJoinInfo { get; set; }

		/// <summary>Gets or sets the value indicating whether to show the social share buttons on the registration page.</summary>
		[JsonPropertyName("show_share_button")]
		public bool? ShowSocialShareButtons { get; set; }

		/// <summary>Gets or sets the URL of the survey displayed in attendees' browsers after leaving the webinar.</summary>
		[JsonPropertyName("survey_url")]
		public string SurveyUrl { get; set; }

		/// <summary>Gets or sets the value indicating whether to use Personal Meeting ID. Only used for scheduled webinars and recurring webinars with no fixed time.</summary>
		[JsonPropertyName("use_pmi")]
		public bool? UsePmi { get; set; }
	}
}
