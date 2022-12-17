using Pathoschild.Http.Client.Formatters;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	internal class AppJsonFormatter : JsonFormatter
	{
		static AppJsonFormatter()
		{
			SerializerOptions = new JsonSerializerOptions()
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				Converters =
				{
					new DateTimeConverter(),
					new NullableDateTimeConverter(),
					new DayOfWeekConverter(),
					new DaysOfWeekConverter(),
					new MeetingConverter(),
					new ParticipantDeviceConverter(),
					new WebHookEventConverter(),
					new WebinarConverter(),
					new BooleanConverter(),
					new EnumConverterFactory()
				}
			};

			DeserializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = false,
				Converters =
				{
					new DateTimeConverter(),
					new NullableDateTimeConverter(),
					new DayOfWeekConverter(),
					new DaysOfWeekConverter(),
					new MeetingConverter(),
					new ParticipantDeviceConverter(),
					new WebHookEventConverter(),
					new WebinarConverter(),
					new BooleanConverter(),
					new EnumConverterFactory()
				}
			};

			SerializationContext = new ZoomNetAppJsonSerializerContext(SerializerOptions);
			DeserializationContext = new ZoomNetAppJsonSerializerContext(DeserializerOptions);
		}
	}
}
