using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Follow-up email notification settings.</summary>
	public class FollowupEmailNotificationSettings
	{
		/// <summary>Gets or sets a value indicating whether a follow-up email notification should be sent to attendees and panelists.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>Gets or sets the schedule of follow-up email notification(s).</summary>
		[JsonPropertyName("type")]
		public FollowupEmailNotificationSchedule Schedule { get; set; }
	}
}
