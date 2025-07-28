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
					new BooleanConverter(),
					new DateTimeConverter(),
					new DayOfWeekConverter(),
					new EnumConverterFactory(),
					new MeetingConverter(),
					new NullableDateTimeConverter(),
					new NullableDayOfWeekConverter(),
					new ParticipantDeviceConverter(),
					new WebHookEventConverter(),
					new WebinarConverter(),
					new EventConverter(),
					new InterpreterConverter(),
				}
			};

			DeserializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = false,
				Converters =
				{
					new BooleanConverter(),
					new DateTimeConverter(),
					new DayOfWeekConverter(),
					new EnumConverterFactory(),
					new MeetingConverter(),
					new NullableDateTimeConverter(),
					new NullableDayOfWeekConverter(),
					new ParticipantDeviceConverter(),
					new WebHookEventConverter(),
					new WebinarConverter(),
					new EventConverter(),
					new InterpreterConverter(),
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
				CleanJsonObject((JsonObject)value);
			}
			else if (type == typeof(JsonObject[]))
			{
				foreach (var jsonObject in (JsonObject[])value)
				{
					CleanJsonObject(jsonObject);
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

		/*
			When upgrading to .NET 6.0, I discovered that serializing a JsonObject does NOT respect 'JsonIgnoreCondition.WhenWritingNull'.
			This is a documented shortcoming that is currently considered "as designed" by the .NET team.
			See: https://github.com/dotnet/runtime/issues/54184 and https://github.com/dotnet/docs/issues/27824

			This behavior has not changed in .NET 7.0.

			Microsoft made the decision to keep the behavior but improve the documentation to ensure developers are aware of it:
			https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-dom#jsonnode-with-jsonserializeroptions

			That's why I wrote the following workaround which removes properties that contain a null value prior to serializing the JsonObject.
		*/
		private static void CleanJsonObject(JsonObject jsonObject)
		{
			var nullProperties = jsonObject
				.Where(kvp => kvp.Value == null)
				.Select(kvp => kvp.Key)
				.ToArray();
			foreach (var propertyName in nullProperties)
			{
				jsonObject.Remove(propertyName);
			}

			var jsonObjectProperties = jsonObject
				.Where(kvp => kvp.Value is JsonObject)
				.Select(kvp => kvp.Value as JsonObject)
				.ToArray();
			foreach (var jsonObjectPropertyValue in jsonObjectProperties)
			{
				CleanJsonObject(jsonObjectPropertyValue);
			}

			var arrayOfJsonObjectsProperties = jsonObject
				.Where(kvp => kvp.Value is JsonValue)
				.Select(kvp =>
				{
					if (kvp.Value.AsValue().TryGetValue<JsonObject[]>(out JsonObject[] arrayOfJsonObjects))
					{
						return arrayOfJsonObjects;
					}

					return null;
				})
				.Where(v => v is not null)
				.ToArray();
			foreach (var arrayOfJsonObjectsPropertyValue in arrayOfJsonObjectsProperties)
			{
				foreach (var item in arrayOfJsonObjectsPropertyValue)
				{
					CleanJsonObject(item);
				}
			}
		}
	}
}
