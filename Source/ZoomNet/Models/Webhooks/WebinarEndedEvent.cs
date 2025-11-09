using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a webinar has ended.
	/// </summary>
	public class WebinarEndedEvent : WebinarInfoEvent
	{
		/// <summary>
		/// Gets or sets the date and time when the webinar ended.
		/// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the webinar is a practice session.
		/// </summary>
		public bool PracticeSession { get; set; }
	}
}
