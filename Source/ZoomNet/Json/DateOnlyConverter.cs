using System;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a DateOnly (which is represented by 3 integer values: year, month and day) to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class DateOnlyConverter : ZoomNetJsonConverter<(int Year, int Month, int Day)>
	{
		public override (int Year, int Month, int Day) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
				case JsonTokenType.String when string.IsNullOrEmpty(reader.GetString()):
					throw new JsonException("Unable to convert a null value to DateOnly");

				case JsonTokenType.String:
					var rawValue = reader.GetString();
					var parts = rawValue.Split('-');
					if (parts.Length != 3) throw new JsonException($"Unable to convert {rawValue} to DateOnly");
					return (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));

				default:
					throw new JsonException($"Unable to convert {reader.TokenType.ToEnumString()} to DateOnly");
			}
		}

		public override void Write(Utf8JsonWriter writer, (int Year, int Month, int Day) value, JsonSerializerOptions options)
		{
			writer.WriteStringValue($"{value.Year:D4}-{value.Month:D2}-{value.Day:D2}");
		}
	}
}
