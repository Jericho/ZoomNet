using System.Text.Json;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>Represents an event of 'unknown' type.</summary>
	public record UnknownEvent : Event
	{
		/// <summary>Gets or sets the name of the event type.</summary>
		public string EventTypeName { get; init; }

		/// <summary>Gets or sets the JSON payload.</summary>
		public JsonElement Payload { get; init; }
	}
}
