using Newtonsoft.Json;
using ZoomNet.Resources;

namespace ZoomNet.Models
{
	/// <summary>
	/// Webinar Settings.
	/// </summary>
	public class WebinarSettings
	{
		/// <summary>
		/// Gets or sets the value indicating whether to start video when host joins the webinar.
		/// </summary>
		[JsonProperty(PropertyName = "host_video")]
		public bool? StartVideoWhenHostJoins { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to start video when panelists join the webinar.
		/// </summary>
		[JsonProperty(PropertyName = "panelists_video")]
		public bool? StartVideoWhenPanelistsJoin { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to enable practice session.
		/// </summary>
		[JsonProperty(PropertyName = "practice_session")]
		public bool? EnablePracticeSession { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to enable HD video.
		/// </summary>
		[JsonProperty(PropertyName = "hd_video")]
		public bool? EnableHighDefinitionVideo { get; set; }

		/// <summary>
		/// Gets or sets the approval type.
		/// </summary>
		[JsonProperty(PropertyName = "approval_type")]
		public MeetingApprovalType? ApprovalType { get; set; }

		/// <summary>
		/// Gets or sets the registration type. Used for recurring meeting with fixed time only.
		/// </summary>
		[JsonProperty(PropertyName = "registration_type")]
		public MeetingRegistrationType? RegistrationType { get; set; }

		/// <summary>
		/// Gets or sets the value indicating how participants can join the audio portion of the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "audio")]
		public AudioType? Audio { get; set; }

		/// <summary>
		/// Gets or sets the value indicating if audio is recorded and if so, when the audio is saved.
		/// </summary>
		[JsonProperty(PropertyName = "auto_recording")]
		public RecordingType AutoRecording { get; set; }

		/// <summary>
		/// Gets or sets the value indicating that only signed-in users can join this meeting.
		/// </summary>
		[JsonProperty(PropertyName = "enforce_login")]
		public bool? EnforceLogin { get; set; }

		/// <summary>
		/// Gets or sets the value indicating only signed-in users with specified domains can join this meeting.
		/// </summary>
		[JsonProperty(PropertyName = "enforce_login_domains")]
		public string EnforceLoginDomains { get; set; }

		/// <summary>
		/// Gets or sets the value indicating alternative hosts emails or IDs. Multiple value separated by comma.
		/// </summary>
		[JsonProperty(PropertyName = "alternative_hosts")]
		public string AlternativeHosts { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether registration is closed after event date.
		/// </summary>
		[JsonProperty(PropertyName = "close_registration")]
		public bool? CloseRegistration { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to show the social share buttons on the registration page.
		/// </summary>
		[JsonProperty(PropertyName = "show_share_button")]
		public bool? ShowSocialShareButtons { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to allow attendees to join from multiple devices.
		/// </summary>
		[JsonProperty(PropertyName = "allow_multiple_devices")]
		public bool? AllowMultipleDevices { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to make the webinar on-demand.
		/// </summary>
		[JsonProperty(PropertyName = "on_demand")]
		public bool? OnDemand { get; set; }

		/// <summary>
		/// Gets or sets the list of global dial-in countries.
		/// </summary>
		[JsonProperty(PropertyName = "global_dial_in_countries")]
		public string[] GlobalDialInCountries { get; set; }

		/// <summary>
		/// Gets or sets the contact name for registration.
		/// </summary>
		[JsonProperty(PropertyName = "contact_name")]
		public string ContactName { get; set; }

		/// <summary>
		/// Gets or sets the contact email for registration.
		/// </summary>
		[JsonProperty(PropertyName = "contact_email")]
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of registrants.
		/// </summary>
		/// <remarks>
		/// Omitting this value, setting it to a negative value or setting it to zero indicates that the number of registrants will not be restricted.
		/// </remarks>
		[JsonProperty(PropertyName = "registrants_restrict_number")]
		public int? MaximumNumberOfRegistrants { get; set; }

		/// <summary>
		/// Gets or sets the URL of the survey displayed in attendees' browsers after leaving the webinar.
		/// </summary>
		[JsonProperty(PropertyName = "survey_url")]
		public string SurveyUrl { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether email notifications are sent about approval, cancellation, denial of registration.
		/// </summary>
		[JsonProperty(PropertyName = "registrants_email_notification")]
		public bool? SendRegistrationConfirmationEmail { get; set; }

		/// <summary>
		/// Gets or sets the value indicating that only authenticated users can join the webinar.
		/// </summary>
		[JsonProperty(PropertyName = "meeting_authentication")]
		public bool? AuthenticatedUsersOnly { get; set; }

		/// <summary>
		/// Gets or sets the authentication type for users to join a webinar when <see cref="AuthenticatedUsersOnly"/> is set to true.
		/// </summary>
		/// <remarks>
		/// The value of this field can be retrieved from the <see cref="AuthenticationOptions.Id"/> in the response of <see cref="IUsers.GetMeetingAuthenticationSettingsAsync"/>.
		/// </remarks>
		[JsonProperty(PropertyName = "authentication_option")]
		public string AuthenticationType { get; set; }
	}
}
