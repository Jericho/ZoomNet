using System;
using System.Globalization;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="DateTime"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class DateTimeConverter : ZoomNetJsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
				case JsonTokenType.String when string.IsNullOrEmpty(reader.GetString()):
					throw new JsonException($"Unable to convert a null value to DateTime");

				// https://devforum.zoom.us/t/webhook-documentation-shows-unusual-date-time-format/147462
				case JsonTokenType.String when reader.GetString().StartsWith("GMT", StringComparison.OrdinalIgnoreCase):
					var dateTimeString = reader.GetString().Remove(0, 3);
					return DateTime.ParseExact(dateTimeString, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

				case JsonTokenType.String:
					return reader.GetDateTime();

				default:
					throw new JsonException($"Unable to convert {reader.TokenType.ToString()} to DateTime");
			}
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToZoomFormat(TimeZones.UTC));
		}
	}
}
