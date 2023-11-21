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
	internal class JsonFormatter : MediaTypeFormatterBase
	{
		internal static readonly JsonSerializerOptions SerializerOptions;
		internal static readonly JsonSerializerOptions DeserializerOptions;

		internal static readonly ZoomNetJsonSerializerContext SerializationContext;
		internal static readonly ZoomNetJsonSerializerContext DeserializationContext;

		private const int DefaultBufferSize = 1024;

		static JsonFormatter()
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

			SerializationContext = new ZoomNetJsonSerializerContext(SerializerOptions);
			DeserializationContext = new ZoomNetJsonSerializerContext(DeserializerOptions);
		}

		public JsonFormatter()
		{
			this.AddMediaType("application/json");
		}

		public override object Deserialize(Type type, Stream stream, HttpContent content, IFormatterLogger formatterLogger)
		{
			var reader = new StreamReader(
				stream,
				encoding: Encoding.UTF8,
				detectEncodingFromByteOrderMarks: true,
				bufferSize: DefaultBufferSize,
				leaveOpen: true); // don't close (stream disposal is handled elsewhere)
			string streamContent = reader.ReadToEnd();
			object deserializedResult = JsonSerializer.Deserialize(streamContent, type, DeserializationContext);
			return deserializedResult;
		}

		public override void Serialize(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
		{
			if (type == typeof(JsonObject))
			{
				/*
					When upgrading to .NET 6.0, I discovered that serializing a JsonObject does NOT respect 'JsonIgnoreCondition.WhenWritingNull'.
					This is a documented shortcoming that is currently considered "as designed" by the .NET team.
					See: https://github.com/dotnet/runtime/issues/54184 and https://github.com/dotnet/docs/issues/27824

					This behavior has not changed in .NET 7.0.

					Microsoft made the decision to keep the behavior but improve the documentation to ensure developers are aware of it:
					https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-dom#jsonnode-with-jsonserializeroptions

					That's why I wrote the following workaround which removes properties that contain a null value prior to serializing the JsonObject.
				*/

				var valueAsJsonObject = (JsonObject)value;
				var nullProperties = valueAsJsonObject
					.Where(kvp => kvp.Value == null)
					.Select(kvp => kvp.Key)
					.ToArray();
				foreach (var propertyName in nullProperties)
				{
					valueAsJsonObject.Remove(propertyName);
				}
			}

			var writer = new StreamWriter(
				stream,
				encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true),
				bufferSize: DefaultBufferSize,
				leaveOpen: true);
			writer.Write(JsonSerializer.Serialize(value, type, SerializationContext));
			writer.Flush();
		}
	}
}
