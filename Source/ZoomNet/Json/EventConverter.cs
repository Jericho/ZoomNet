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

			var eventType = rootElement.GetPropertyValue<string>("event_type").ToEnum<EventType>();

			switch (eventType)
			{
				case EventType.Simple:
					return rootElement.ToObject<SimpleEvent>(options);
				case EventType.Conference:
					return rootElement.ToObject<Conference>(options);
				default:
					throw new JsonException($"{eventType} is an unknown event type");
			}
		}
	}
}
