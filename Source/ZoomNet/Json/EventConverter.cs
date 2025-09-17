using System;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="Event">event</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class EventConverter : ZoomNetJsonConverter<Event>
	{
		public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var rawEventType = rootElement.GetPropertyValue<string>("event_type");
			if (!rawEventType.TryToEnum(out EventType eventType)) throw new JsonException($"{rawEventType} is an unknown event type");

			switch (eventType)
			{
				case EventType.Simple:
					return rootElement.ToObject<SimpleEvent>(options);
				case EventType.Conference:
					return rootElement.ToObject<Conference>(options);
				case EventType.Reccuring:
					return rootElement.ToObject<RecurringEvent>(options);
				default:
					throw new JsonException($"{eventType} is an unknown event type");
			}
		}
	}
}
