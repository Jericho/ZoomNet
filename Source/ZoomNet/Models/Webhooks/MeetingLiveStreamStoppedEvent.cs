using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting live stream stopped.
	/// </summary>
	public class MeetingLiveStreamStoppedEvent : MeetingLiveStreamEvent
	{
		/// <summary>
		/// Gets or sets the date and time at which the live stream stopped.
		/// </summary>
		public DateTime StoppedOn { get; set; }
	}
}
