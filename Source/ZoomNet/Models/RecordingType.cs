namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of recorded meeting or webinar.
	/// </summary>
	public enum RecordingType
	{
		/// <summary>
		/// Instant meeting.
		/// </summary>
		InstantMeeting = 1,

		/// <summary>
		/// Scheduled meeting.
		/// </summary>
		ScheduledMeeting = 2,

		/// <summary>
		/// A recurring meeting with no fixed time.
		/// </summary>
		RecurringMeetingNoFixedTime = 3,

		/// <summary>
		/// A meeting created via PMI (Personal Meeting ID).
		/// </summary>
		PersonnalMeeting = 4,

		/// <summary>
		/// A webinar.
		/// </summary>
		Webinar = 5,

		/// <summary>
		/// A recurring webinar without a fixed time.
		/// </summary>
		RecurringWebinarNoFixedTime = 6,

		/// <summary>
		/// A Personal Audio Conference (PAC).
		/// </summary>
		PersonalAudioConference = 7,

		/// <summary>
		/// Recurring meeting with a fixed time.
		/// </summary>
		RecurringMeetingFixedTime = 8,

		/// <summary>
		/// A recurring webinar with a fixed time.
		/// </summary>
		RecurringWebinarFixedTime = 9,

		/// <summary>
		/// A recording uploaded via the Recordings interface on the Zoom Web Portal.
		/// </summary>
		ManuallyUploaded = 99
	}
}
