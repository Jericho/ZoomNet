using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Reminder settings.</summary>
	public record ReminderSettings
	{
		/// <summary>Gets or sets a value indicating whether a reminder should be sent to attendees and panelists.</summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; init; }

		/// <summary>Gets or sets the schedule of the reminder.</summary>
		[JsonPropertyName("type")]
		public ReminderSchedule Schedule { get; init; }
	}
}
