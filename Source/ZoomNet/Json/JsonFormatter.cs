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
		internal static readonly JsonSerializerOptions DefaultSerializerOptions;
		internal static readonly JsonSerializerOptions DefaultDeserializerOptions;

		internal static readonly ZoomNetJsonSerializerContext DefaultSerializationContext;
		internal static readonly ZoomNetJsonSerializerContext DefaultDeserializationContext;

		private const int DefaultBufferSize = 1024;

		private JsonSerializerOptions _serializerOptions;
		private JsonSerializerOptions _deserializerOptions;

		private ZoomNetJsonSerializerContext _serializationContext;
		private ZoomNetJsonSerializerContext _deserializationContext;

		public JsonSerializerOptions SerializerOptions
		{
			get
			{
				if (_serializerOptions == null)
				{
					_serializerOptions = new JsonSerializerOptions
					{
						AllowTrailingCommas = DefaultSerializerOptions.AllowTrailingCommas,
						DefaultIgnoreCondition = DefaultSerializerOptions.DefaultIgnoreCondition,
						DefaultBufferSize = DefaultSerializerOptions.DefaultBufferSize,
						DictionaryKeyPolicy = DefaultSerializerOptions.DictionaryKeyPolicy,
						Encoder = DefaultSerializerOptions.Encoder,
						IgnoreReadOnlyFields = DefaultSerializerOptions.IgnoreReadOnlyFields,
						IgnoreReadOnlyProperties = DefaultSerializerOptions.IgnoreReadOnlyProperties,
						IncludeFields = DefaultSerializerOptions.IncludeFields,
						MaxDepth = DefaultSerializerOptions.MaxDepth,
						NumberHandling = DefaultSerializerOptions.NumberHandling,
						PreferredObjectCreationHandling = DefaultSerializerOptions.PreferredObjectCreationHandling,
						PropertyNameCaseInsensitive = DefaultSerializerOptions.PropertyNameCaseInsensitive,
						PropertyNamingPolicy = DefaultSerializerOptions.PropertyNamingPolicy,
						ReadCommentHandling = DefaultSerializerOptions.ReadCommentHandling,
						ReferenceHandler = DefaultSerializerOptions.ReferenceHandler,
						TypeInfoResolver = DefaultSerializerOptions.TypeInfoResolver,
						UnknownTypeHandling = DefaultSerializerOptions.UnknownTypeHandling,
						UnmappedMemberHandling = DefaultSerializerOptions.UnmappedMemberHandling,
						WriteIndented = DefaultSerializerOptions.WriteIndented,
					};

					foreach (var converter in DefaultSerializerOptions.Converters)
					{
						_serializerOptions.Converters.Add(converter);
					}
				}

				return _serializerOptions;
			}
		}

		public JsonSerializerOptions DeserializerOptions
		{
			get
			{
				if (_deserializerOptions == null)
				{
					_deserializerOptions = new JsonSerializerOptions
					{
						AllowTrailingCommas = DefaultDeserializerOptions.AllowTrailingCommas,
						DefaultIgnoreCondition = DefaultDeserializerOptions.DefaultIgnoreCondition,
						DefaultBufferSize = DefaultDeserializerOptions.DefaultBufferSize,
						DictionaryKeyPolicy = DefaultDeserializerOptions.DictionaryKeyPolicy,
						Encoder = DefaultDeserializerOptions.Encoder,
						IgnoreReadOnlyFields = DefaultDeserializerOptions.IgnoreReadOnlyFields,
						IgnoreReadOnlyProperties = DefaultDeserializerOptions.IgnoreReadOnlyProperties,
						IncludeFields = DefaultDeserializerOptions.IncludeFields,
						MaxDepth = DefaultDeserializerOptions.MaxDepth,
						NumberHandling = DefaultDeserializerOptions.NumberHandling,
						PreferredObjectCreationHandling = DefaultDeserializerOptions.PreferredObjectCreationHandling,
						PropertyNameCaseInsensitive = DefaultDeserializerOptions.PropertyNameCaseInsensitive,
						PropertyNamingPolicy = DefaultDeserializerOptions.PropertyNamingPolicy,
						ReadCommentHandling = DefaultDeserializerOptions.ReadCommentHandling,
						ReferenceHandler = DefaultDeserializerOptions.ReferenceHandler,
						TypeInfoResolver = DefaultDeserializerOptions.TypeInfoResolver,
						UnknownTypeHandling = DefaultDeserializerOptions.UnknownTypeHandling,
						UnmappedMemberHandling = DefaultDeserializerOptions.UnmappedMemberHandling,
						WriteIndented = DefaultDeserializerOptions.WriteIndented,
					};

					foreach (var converter in DefaultDeserializerOptions.Converters)
					{
						_deserializerOptions.Converters.Add(converter);
					}
				}

				return _deserializerOptions;
			}
		}

		public ZoomNetJsonSerializerContext SerializationContext
		{
			get
			{
				if (_serializationContext == null) _serializationContext = new ZoomNetJsonSerializerContext(SerializerOptions);
				return _serializationContext;
			}
		}

		public ZoomNetJsonSerializerContext DeserializationContext
		{
			get
			{
				if (_deserializationContext == null) _deserializationContext = new ZoomNetJsonSerializerContext(DeserializerOptions);
				return _deserializationContext;
			}
		}

		static JsonFormatter()
		{
			DefaultSerializerOptions = new JsonSerializerOptions()
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				Converters =
				{
					new BooleanConverter(),
					new DateTimeConverter(),
					new DayOfWeekConverter(),
					new DaysOfWeekConverter(),
					new EnumConverterFactory(),
					new MeetingConverter(),
					new NullableDateTimeConverter(),
					new NullableDayOfWeekConverter(),
					new ParticipantDeviceConverter(),
					new WebinarConverter(),
					new EventConverter(),
					new InterpreterConverter(),
					new ContactCenterSystemStatusConverter(),
				}
			};

			DefaultDeserializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = false,
				Converters =
				{
					new BooleanConverter(),
					new DateTimeConverter(),
					new DayOfWeekConverter(),
					new DaysOfWeekConverter(),
					new EnumConverterFactory(),
					new MeetingConverter(),
					new NullableDateTimeConverter(),
					new NullableDayOfWeekConverter(),
					new ParticipantDeviceConverter(),
					new WebinarConverter(),
					new EventConverter(),
					new InterpreterConverter(),
					new ContactCenterSystemStatusConverter(),
				}
			};

			DefaultSerializationContext = new ZoomNetJsonSerializerContext(DefaultSerializerOptions);
			DefaultDeserializationContext = new ZoomNetJsonSerializerContext(DefaultDeserializerOptions);
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
			// STEP 1 - Remove properties containing a null value
			var nullProperties = jsonObject
				.Where(kvp => kvp.Value == null)
				.Select(kvp => kvp.Key)
				.ToArray();
			foreach (var propertyName in nullProperties)
			{
				jsonObject.Remove(propertyName);
			}

			// STEP 2 - Recursively process properties containing a JsonObject
			var jsonObjectProperties = jsonObject
				.Where(kvp => kvp.Value is JsonObject)
				.Select(kvp => kvp.Value as JsonObject)
				.ToArray();
			foreach (var jsonObjectPropertyValue in jsonObjectProperties)
			{
				CleanJsonObject(jsonObjectPropertyValue);
			}

			// STEP 3 - Recursively process properties containing an array of JsonObjects
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

			// STEP 4 - Remove properties containing an empty JsonObject
			var emptyJsonObjectProperties = jsonObject
				.Where(kvp => kvp.Value is JsonObject)
				.Where(kvp => (kvp.Value as JsonObject).Count == 0)
				.Select(kvp => kvp.Key)
				.ToArray();
			foreach (var propertyName in emptyJsonObjectProperties)
			{
				jsonObject.Remove(propertyName);
			}
		}
	}
}
