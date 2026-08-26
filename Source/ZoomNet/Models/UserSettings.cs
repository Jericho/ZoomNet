using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>User settings.</summary>
	public class UserSettings
	{
		/// <summary>Gets or sets the settings for scheduledaudio conferencing.</summary>
		[JsonPropertyName("audio_conferencing")]
		public AudioConferencingUserSettings AudioConferencing { get; set; }

		/// <summary>Gets or sets the settings for email notification.</summary>
		[JsonPropertyName("email_notification")]
		public EmailNotificationUserSettings EmailNotification { get; set; }

		/// <summary>Gets or sets the settings for feature.</summary>
		[JsonPropertyName("feature")]
		public FeatureUserSettings Feature { get; set; }

		/// <summary>Gets or sets the settings for instant meetings.</summary>
		[JsonPropertyName("in_meeting")]
		public InstantMeetingUserSettings InstantMeeting { get; set; }

		/// <summary>Gets or sets the user profile.</summary>
		[JsonPropertyName("profile")]
		public UserProfile Profile { get; set; }

		/// <summary>Gets or sets the settings for recordings.</summary>
		[JsonPropertyName("recording")]
		public RecordingUserSettings Recordings { get; set; }

		/// <summary>Gets or sets the settings for scheduled meetings.</summary>
		[JsonPropertyName("schedule_meeting")]
		public ScheduledMeetingUserSettings ScheduledMeeting { get; set; }

		/// <summary>Gets or sets the settings for telephony.</summary>
		[JsonPropertyName("telephony")]
		public TelephonyUserSettings Telephony { get; set; }

		/// <summary>Gets or sets the settings for TSP.</summary>
		[JsonPropertyName("tsp")]
		public TspUserSettings Tsp { get; set; }
	}
}
