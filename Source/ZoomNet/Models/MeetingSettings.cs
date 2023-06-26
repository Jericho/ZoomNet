using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting Settings.
	/// </summary>
	public class MeetingSettings
	{
		/// <summary>
		/// Gets or sets the value indicating alternative hosts email addresses or IDs. Multiple value separated by semicolon.
		/// </summary>
		[JsonPropertyName("alternative_hosts")]
		public string AlternativeHosts { get; set; }

		/// <summary>
		/// Gets or sets the approval type.
		/// </summary>
		[JsonPropertyName("approval_type")]
		public ApprovalType? ApprovalType { get; set; }

		/// <summary>
		/// Gets or sets the value indicating how participants can join the audio portion of the meeting.
		/// </summary>
		[JsonPropertyName("audio")]
		public AudioType? Audio { get; set; }

		/// <summary>
		/// Gets or sets the value indicating if audio is recorded and if so, where the audio is saved.
		/// </summary>
		[JsonPropertyName("auto_recording")]
		public AutoRecordingType? AutoRecording { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether registration is closed after event date.
		/// </summary>
		[JsonPropertyName("close_registration")]
		public bool? CloseRegistration { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the meeting should be hosted in China.
		/// </summary>
		[JsonPropertyName("cn_meeting")]
		[Obsolete("Deprecated")]
		public bool? HostInChina { get; set; }

		/// <summary>
		/// Gets or sets the contact email for registration.
		/// </summary>
		[JsonPropertyName("contact_email")]
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets the contact name for registration.
		/// </summary>
		[JsonPropertyName("contact_name")]
		public string ContactName { get; set; }

		/// <summary>
		/// Gets or sets the value indicating that only signed-in users can join this meeting.
		/// </summary>
		[JsonPropertyName("enforce_login")]
		[Obsolete("This field is deprecated and will not be supported in the future.")]
		public bool? EnforceLogin { get; set; }

		/// <summary>
		/// Gets or sets the value indicating only signed-in users with specified domains can join this meeting.
		/// </summary>
		[JsonPropertyName("enforce_login_domains")]
		[Obsolete("This field is deprecated and will not be supported in the future.")]
		public string EnforceLoginDomains { get; set; }

		/// <summary>
		/// Gets or sets the list of global dial-in countries.
		/// </summary>
		[JsonPropertyName("global_dial_in_countries")]
		public string[] GlobalDialInCountries { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the 'Allow host to save video order' feature is enabled.
		/// </summary>
		[JsonPropertyName("host_save_video_order")]
		public bool? HostCanSaveVideoOrder { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to start video when host joins the meeting.
		/// </summary>
		[JsonPropertyName("host_video")]
		public bool? StartVideoWhenHostJoins { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the meeting should be hosted in India.
		/// </summary>
		[JsonPropertyName("in_meeting")]
		[Obsolete("Deprecated")]
		public bool? HostInIndia { get; set; }

		/// <summary>
		/// Gets or sets the time limits within which a participant can join a meeting before the meeting's host.
		/// </summary>
		/// <remarks>This value is applicable only if <see cref="JoinBeforeHost"/> is true.</remarks>
		[JsonPropertyName("jbh_time")]
		public JoinBeforeHostTime? JoinBeforeHostTime { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether participants can join the meeting before the host starts the meeting. Only used for scheduled or recurring meetings.
		/// </summary>
		[JsonPropertyName("join_before_host")]
		public bool? JoinBeforeHost { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether participants are muted upon entry.
		/// </summary>
		[JsonPropertyName("mute_upon_entry")]
		public bool? MuteUponEntry { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to start video when participants join the meeting.
		/// </summary>
		[JsonPropertyName("participant_video")]
		public bool? StartVideoWhenParticipantsJoin { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether a confirmation email is sent when a participant registers.
		/// </summary>
		[JsonPropertyName("registrants_confirmation_email")]
		public bool? SendRegistrationConfirmationEmail { get; set; }

		/// <summary>
		/// Gets or sets the registration type. Used for recurring meeting with fixed time only.
		/// </summary>
		[JsonPropertyName("registration_type")]
		public RegistrationType? RegistrationType { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to ask the permission to unmute partecipants.
		/// </summary>
		[JsonPropertyName("request_permission_to_unmute_participants")]
		public bool? RequestPermissionToUnmutePartecipants { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to use Personal Meeting ID. Only used for scheduled meetings and recurring meetings with no fixed time.
		/// </summary>
		[JsonPropertyName("use_pmi")]
		public bool? UsePmi { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to use a waiting room.
		/// </summary>
		[JsonPropertyName("waiting_room")]
		public bool? WaitingRoom { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether a watermark should be displayed when viewing shared screen.
		/// </summary>
		[JsonPropertyName("watermark")]
		public bool? Watermark { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to allow attendees to join the meeting from multiple devices.
		/// This setting only works for meetings that require registration.
		/// </summary>
		[JsonPropertyName("allow_multiple_devices")]
		public bool? AllowMultipleDevices { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to send email notifications to alternative hosts, default value is true.
		/// </summary>
		[JsonPropertyName("alternative_hosts_email_notification")]
		public bool? SendNotificationsToAlternativeHosts { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to allow alternative hosts to add or edit polls.
		/// This requires Zoom version 5.8.0 or higher.
		/// </summary>
		[JsonPropertyName("alternative_host_update_polls")]
		public bool? AllowAlternativeHostToUpdatePolls { get; set; }

		/// <summary>
		/// Gets or sets the third party audio conference info.
		/// Constraints: Max 2048 chars.
		/// </summary>
		[JsonPropertyName("audio_conference_info")]
		public string ThirdPartyAudioConferenceInfo { get; set; }

		/// <summary>
		/// Gets or sets the list the domains that are authenticated if user has configured "Sign Into Zoom with Specified Domains" option.
		/// </summary>
		[JsonPropertyName("authentication_domains")]
		public string AuthenticationDomains { get; set; }

		/// <summary>
		/// Gets or sets the authentication name set in the authentication profile.
		/// </summary>
		[JsonPropertyName("authentication_name")]
		public string AuthenticationName { get; set; }

		/// <summary>
		/// Gets or Sets the authentication option id.
		/// </summary>
		[JsonPropertyName("authentication_option")]
		public string AuthenticationOptionId { get; set; }

		/// <summary>
		/// Gets or sets the type of calendar integration used to schedule the meeting.
		/// Works with the private_meeting field to determine whether to share details of meetings or not.
		/// </summary>
		[JsonPropertyName("calendar_type")]
		public CalendarIntegrationType? CalendarIntegrationType { get; set; }

		/// <summary>
		/// Gets or sets the custom keys and values assigned to the meeting.
		/// </summary>
		[JsonPropertyName("custom_keys")]
		[JsonConverter(typeof(KeyValuePairConverter))]
		public KeyValuePair<string, string>[] CustomKeys { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to send email notifications to alternative hosts and users with <a href="https://support.zoom.us/hc/en-us/articles/201362803-Scheduling-privilege">scheduling privileges</a>.
		/// This value defaults to true.
		/// </summary>
		[JsonPropertyName("email_notification")]
		public bool? SendEmailNotifications { get; set; }

		/// <summary>
		/// Gets or sets the encryption type.
		/// When using end-to-end encryption, several features (e.g. cloud recording, phone/SIP/H.323 dial-in) will be automatically disabled.
		/// </summary>
		[JsonPropertyName("encryption_type")]
		public EncryptionType? EncryptionType { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the Focus Mode feature is enabled when the meeting starts.
		/// </summary>
		[JsonPropertyName("focus_mode")]
		public bool? FocusModeEnabled { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether only authenticated users can join meetings.
		/// </summary>
		[JsonPropertyName("meeting_authentication")]
		public bool? AuthenticationRequired { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the meeting is set as private.
		/// </summary>
		[JsonPropertyName("private_meeting")]
		public bool? Private { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to send registrants an email confirmation.
		/// </summary>
		[JsonPropertyName("registrants_email_notification")]
		public bool? SendConfirmationEmailToRegistrants { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to show social share buttons on the meeting registration page.
		/// This setting only works for meetings that require registration.
		/// </summary>
		[JsonPropertyName("show_share_button")]
		public bool? ShowShareButton { get; set; }
	}
}
