using System.Text.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Represents an event of 'unknown' type.
	/// </summary>
	public class UnknownEvent : Event
	{
		/// <summary>
		/// Gets or sets the name of the event type.
		/// </summary>
		public string EventTypeName { get; set; }

		/// <summary>
		/// Gets or sets the JSON payload.
		/// </summary>
		public JsonElement Payload { get; set; }
	}
}
