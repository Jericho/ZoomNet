using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZoomNet.Models;

namespace ZoomNet.Utilities.Json
{
	/// <summary>
	/// Converts a JSON string into a <see cref="Webinar">webinar</see>.
	/// </summary>
	/// <seealso cref="JsonConverter" />
	internal class WebinarConverter : JsonConverter<Webinar>
	{
		public override Webinar Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var webinarType = rootElement.GetPropertyValue<WebinarType>("type");

			switch (webinarType)
			{
				case WebinarType.Regular:
					return rootElement.ToObject<ScheduledWebinar>(options);
				case WebinarType.RecurringFixedTime:
				case WebinarType.RecurringNoFixedTime:
					return rootElement.ToObject<RecurringWebinar>(options);
				default:
					throw new Exception($"{webinarType} is an unknown webinar type");
			}
		}

		public override void Write(Utf8JsonWriter writer, Webinar value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
