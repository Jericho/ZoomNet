using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting has ended.
	/// </summary>
	public class MeetingEndedEvent : MeetingInfoEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the meeting ended.
		/// </summary>
		public DateTime EndTime { get; set; }
	}
}
