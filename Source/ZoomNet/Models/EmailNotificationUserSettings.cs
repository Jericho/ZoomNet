using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Email notification user settings.
	/// </summary>
	public class EmailNotificationUserSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent when an alternative host is set or removed from a meeting.
		/// </summary>
		[JsonPropertyName("alternative_host_reminder")]
		public bool ChangeAlternativeHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent when a meeting is cancelled.
		/// </summary>
		[JsonPropertyName("cancel_meeting_reminder")]
		public bool CancelMeeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent to the host when a cloud recording is available.
		/// </summary>
		[JsonPropertyName("cloud_recording_available_reminder")]
		public bool CloudRecordingAvailableToHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent when attendees join a meeting before the host.
		/// </summary>
		[JsonPropertyName("jbh_reminder")]
		public bool AttendeesJoinBeforeHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent to any alternative hosts when a cloud recording is available.
		/// </summary>
		[JsonPropertyName("recording_available_reminder_alternative_hosts")]
		public bool CloudRecordingAvailableToAlternateHosts { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent to the person who scheduled the meeting or webinar for the host when a cloud recording is available.
		/// </summary>
		[JsonPropertyName("recording_available_reminder_schedulers")]
		public bool CloudRecordingAvailableToScheduler { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent to the host when a meeting is scheduled, rescheduled or cancelled.
		/// </summary>
		[JsonPropertyName("schedule_for_reminder")]
		public bool MeetingScheduled { get; set; }
	}
}
