using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="DayOfWeek">day of the week</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class DayOfWeekConverter : JsonConverter<DayOfWeek>
	{
		public static DayOfWeek FromJsonValue(string value)
		{
			if (!int.TryParse(value, out int intValue) || intValue < 1 || intValue > 7)
			{
				throw new JsonException($"Unable to convert '{value}' into a DayOfWeek value");
			}

			return FromJsonValue(Convert.ToInt32(value));
		}

		public static DayOfWeek FromJsonValue(int value)
		{
			/*
			 * IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
			 * but the values returned by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			 */
			return (DayOfWeek)(value - 1);
		}

		public static int ToJsonValue(DayOfWeek value)
		{
			/*
			 * IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
			 * but the values expected by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			 */
			return Convert.ToInt32(value) + 1;
		}

		public override DayOfWeek Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.Number) throw new JsonException($"Unable to convert {reader.TokenType.ToEnumString()} to DayOfWeek");
			return FromJsonValue(reader.GetInt32());
		}

		public override void Write(Utf8JsonWriter writer, DayOfWeek value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(ToJsonValue(value));
		}
	}
}
