using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Email notification user settings.
	/// </summary>
	public class EmailNotificationUserSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent when attendees join a meeting before the host.
		/// </summary>
		[JsonProperty(PropertyName = "jbh_reminder")]
		public bool AttendeesJoinBeforeHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent when a meeting is cancelled.
		/// </summary>
		[JsonProperty(PropertyName = "cancel_meeting_reminder")]
		public bool CancelMeeting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent when an alternative host is set or removed from a meeting.
		/// </summary>
		[JsonProperty(PropertyName = "alternative_host_reminder")]
		public bool ChangeAlternativeHost { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a notification is sent to the host when a meeting is scheduled, rescheduled or cancelled.
		/// </summary>
		[JsonProperty(PropertyName = "schedule_for_reminder")]
		public bool MeetingScheduled { get; set; }
	}
}
