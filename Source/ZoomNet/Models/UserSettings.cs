using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// User settings.
	/// </summary>
	public class UserSettings
	{
		/// <summary>
		/// Gets or sets the settings for scheduledaudio conferencing.
		/// </summary>
		[JsonPropertyName("audio_conferencing")]
		public AudioConferencingUserSettings AudioConferencing { get; set; }

		/// <summary>
		/// Gets or sets the settings for email notification.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("email_notification")]
		public EmailNotificationUserSettings EmailNotification { get; set; }

		/// <summary>
		/// Gets or sets the settings for feature.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("feature")]
		public FeatureUserSettings Feature { get; set; }

		/// <summary>
		/// Gets or sets the settings for instant meetings.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("in_meeting")]
		public InstantMeetingUserSettings InstantMeeting { get; set; }

		/// <summary>
		/// Gets or sets the user profile.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("profile")]
		public UserProfile Profile { get; set; }

		/// <summary>
		/// Gets or sets the settings for recordings.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("recording")]
		public RecordingUserSettings Recordings { get; set; }

		/// <summary>
		/// Gets or sets the settings for scheduled meetings.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("schedule_meeting")]
		public ScheduledMeetingUserSettings ScheduledMeeting { get; set; }

		/// <summary>
		/// Gets or sets the settings for telephony.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("telephony")]
		public TelephonyUserSettings Telephony { get; set; }

		/// <summary>
		/// Gets or sets the settings for TSP.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonPropertyName("tsp")]
		public TspUserSettings Tsp { get; set; }
	}
}
