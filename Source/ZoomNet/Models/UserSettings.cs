using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// User settings.
	/// </summary>
	public class UserSettings
	{
		/// <summary>
		/// Gets or sets the settings for scheduled meetings.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("schedule_meeting", NullValueHandling = NullValueHandling.Ignore)]
		public ScheduledMeetingUserSettings ScheduledMeeting { get; set; }

		/// <summary>
		/// Gets or sets the settings for instant meetings.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("in_meeting", NullValueHandling = NullValueHandling.Ignore)]
		public InstantMeetingUserSettings InstantMeeting { get; set; }

		/// <summary>
		/// Gets or sets the settings for email notification.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("email_notification", NullValueHandling = NullValueHandling.Ignore)]
		public EmailNotificationUserSettings EmailNotification { get; set; }

		/// <summary>
		/// Gets or sets the settings for recordings.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("recording", NullValueHandling = NullValueHandling.Ignore)]
		public RecordingUserSettings Recordings { get; set; }

		/// <summary>
		/// Gets or sets the settings for telephony.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("telephony", NullValueHandling = NullValueHandling.Ignore)]
		public TelephonyUserSettings Telephony { get; set; }

		/// <summary>
		/// Gets or sets the settings for feature.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("feature", NullValueHandling = NullValueHandling.Ignore)]
		public FeatureUserSettings Feature { get; set; }

		/// <summary>
		/// Gets or sets the settings for TSP.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		[JsonProperty("tsp", NullValueHandling = NullValueHandling.Ignore)]
		public TspUserSettings Tsp { get; set; }
	}
}
