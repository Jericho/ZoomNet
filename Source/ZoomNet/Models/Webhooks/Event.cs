using System;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents a generic event.
	/// </summary>
	public abstract class Event
	{
		/// <summary>
		/// Gets or sets the type of event.
		/// </summary>
		public EventType EventType { get; set; }

		/// <summary>
		/// Gets or sets the timestamp at which the event occured.
		/// </summary>
		/// <remarks>
		/// This field represents the timestamp for when the associated event occurred.
		/// If you keep track of notification delivery timestamp in your application, you
		/// can determine the latency in webhook delivery by calculating the difference
		/// between the delivery timestamp and the value of the event_ts parameter.
		/// </remarks>
		[JsonPropertyName("timestamp")]
		[JsonConverter(typeof(EpochConverter))]
		public DateTime Timestamp { get; set; }
	}
}
