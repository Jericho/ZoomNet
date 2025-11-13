using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event related to meeting or webinar recording progress (started, paused, resumed, stopped).
	/// </summary>
	public abstract class RecordingProgressEvent : RecordingMeetingOrWebinarInfoEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the recording started.
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the recording ended.
		/// </summary>
		public DateTime? EndTime { get; set; }
	}
}
