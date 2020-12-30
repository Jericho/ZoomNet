using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting Settings.
	/// </summary>
	public class MeetingSettings
	{
		/// <summary>
		/// Gets or sets the value indicating whether to start video when host joins the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "host_video")]
		public bool? StartVideoWhenHostJoins { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to start video when participants join the meeting.
		/// </summary>
		[JsonProperty(PropertyName = "participant_video")]
		public bool? StartVideoWhenParticipantsJoin { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the meeting should be hosted in China.
		/// </summary>
		[JsonProperty(PropertyName = "cn_meeting")]
		public bool? HostInChina { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the meeting should be hosted in India.
		/// </summary>
		[JsonProperty(PropertyName = "in_meeting")]
		public bool? HostInIndia { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether participants can join the meeting before the host starts the meeting. Only used for scheduled or recurring meetings.
		/// </summary>
		[JsonProperty(PropertyName = "join_before_host")]
		public bool? JoinBeforeHost { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether participants are muted upon entry.
		/// </summary>
		[JsonProperty(PropertyName = "mute_upon_entry")]
		public bool? MuteUponEntry { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether a watermark should be displayed when viewing shared screen.
		/// </summary>
		[JsonProperty(PropertyName = "watermark")]
		public bool? Watermark { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to use Personal Meeting ID. Only used for scheduled meetings and recurring meetings with no fixed time.
		/// </summary>
		[JsonProperty(PropertyName = "use_pmi")]
		public bool? UsePmi { get; set; }

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
		public RecordingType? AutoRecording { get; set; }

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
		/// Gets or sets the value indicating whether a confirmation email is sent when a participant registers.
		/// </summary>
		[JsonProperty(PropertyName = "registrants_confirmation_email")]
		public bool? SendRegistrationConfirmationEmail { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether to use a waiting room.
		/// </summary>
		[JsonProperty(PropertyName = "waiting_room")]
		public bool? WaitingRoom { get; set; }

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
	}
}
