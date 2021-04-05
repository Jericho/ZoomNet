using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents a generic event.
	/// </summary>
	public class Event
	{
		/// <summary>
		/// Gets or sets the type of event.
		/// </summary>
		public EventType EventType { get; set; }

		/// <summary>
		/// Gets or sets the timestamp at which the event occured.
		/// </summary>
		public DateTime TimeStamp { get; set; }
	}
}
