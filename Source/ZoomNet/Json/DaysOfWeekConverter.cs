using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="DayOfWeek">day of the week</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class DaysOfWeekConverter : ZoomNetJsonConverter<DayOfWeek[]>
	{
		private readonly bool _serializeAsCommaDelimitedString;

		public DaysOfWeekConverter()
			: this(true)
		{
		}

		public DaysOfWeekConverter(bool serializeAsCommaDelimitedString)
		{
			_serializeAsCommaDelimitedString = serializeAsCommaDelimitedString;
		}

		public override DayOfWeek[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return Array.Empty<DayOfWeek>();
			}
			else if (reader.TokenType == JsonTokenType.String)
			{
				var rawValue = reader.GetString();

				return rawValue
					.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(value => DayOfWeekConverter.FromJsonValue(value))
					.ToArray();
			}
			else if (reader.TokenType == JsonTokenType.StartArray)
			{
				var days = new List<DayOfWeek>();
				while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				{
					if (reader.TokenType == JsonTokenType.Number)
					{
						days.Add(DayOfWeekConverter.FromJsonValue(reader.GetInt32()));
					}
				}

				return days.ToArray();
			}
			else
			{
				throw new JsonException($"Unexpected token type: {reader.TokenType}. Unable to convert to Array of DayOfWeek.");
			}
		}

		public override void Write(Utf8JsonWriter writer, DayOfWeek[] value, JsonSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNullValue();
			}
			else if (_serializeAsCommaDelimitedString)
			{
				// Serialize as a comma-delimited string
				var multipleDays = string.Join(",", value.Select(day => DayOfWeekConverter.ToJsonValue(day).ToString()));
				writer.WriteStringValue(multipleDays);
			}
			else
			{
				// Serialize as an array of numbers
				writer.WriteStartArray();

				foreach (var day in value)
				{
					writer.WriteNumberValue(Convert.ToInt32(day) + 1);
				}

				writer.WriteEndArray();
			}
		}
	}
}
