using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static ZoomNet.Internal;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a 'Unix time' expressed as the number of seconds since midnight on January 1st 1970 to and from JSON.
	/// </summary>
	/// <seealso cref="JsonConverter" />
	internal class EpochConverter : ZoomNetJsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var timestamp = reader.TokenType switch
			{
				JsonTokenType.Number => reader.GetInt64(),
				JsonTokenType.String when long.TryParse(reader.GetString(), out var result) => result,
				_ => throw new JsonException($"Unable to convert {reader.TokenType.ToEnumString()} from Epoch to DateTime")
			};

			// Most of the time the value we get from the Zoom API is in "seconds" format but there are a few cases where it's in milliseconds.
			// For instance, the response from "Get API call logs​" includes a field called "time" which is in milliseconds (FYI: the other thing that's particular about this "time" field is that it is a string, not a number).
			// One of the ways to determine the precision of a UNIX epoch field, according to https://www.pythontutorials.net/blog/how-to-test-if-a-given-time-stamp-is-in-seconds-or-milliseconds/#seconds-vs-milliseconds-key-differences,
			// is to check the number of digits.
			var precision = timestamp.GetNumberOfDigits() >= 13 ? UnixTimePrecision.Milliseconds : UnixTimePrecision.Seconds;

			return timestamp.FromUnixTime(precision);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			var timestamp = value.ToUnixTime();
			writer.WriteNumberValue(timestamp);
		}
	}
}
