using Pathoschild.Http.Client.Formatters;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Utilities
{
	internal class ZoomNetJsonFormatter : MediaTypeFormatterBase
	{
		internal static readonly JsonSerializerOptions SerializerOptions;
		internal static readonly JsonSerializerOptions DeserializerOptions;

		internal static readonly ZoomNetJsonSerializerContext SerializationContext;
		internal static readonly ZoomNetJsonSerializerContext DeserializationContext;

		private const int DefaultBufferSize = 1024;

		static ZoomNetJsonFormatter()
		{
			SerializerOptions = new JsonSerializerOptions()
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				Converters =
				{
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

		public ZoomNetJsonFormatter()
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
