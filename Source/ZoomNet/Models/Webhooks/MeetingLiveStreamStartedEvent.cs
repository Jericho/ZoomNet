using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting live stream started.
	/// </summary>
	public class MeetingLiveStreamStartedEvent : MeetingLiveStreamEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the live stream started.
		/// </summary>
		public DateTime StartedOn { get; set; }
	}
}
