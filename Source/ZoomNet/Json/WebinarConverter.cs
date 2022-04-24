using System;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="Webinar">webinar</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class WebinarConverter : ZoomNetJsonConverter<Webinar>
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
	}
}
