using System;
using System.Linq;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a J<see cref="DayOfWeek">days of the week</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class DaysOfWeekConverter : ZoomNetJsonConverter<DayOfWeek[]>
	{
		public override DayOfWeek[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			/*
			 * IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
			 * but the values returned by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			 */

			if (reader.TokenType == JsonTokenType.Null) return null;

			var rawValue = reader.GetString();
			var values = rawValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			return values
				.Select(value => Convert.ToInt32(value) - 1)
				.Select(value => (DayOfWeek)value)
				.ToArray();
		}

		public override void Write(Utf8JsonWriter writer, DayOfWeek[] value, JsonSerializerOptions options)
		{
			/*
			 * IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
			 * but the values returned by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			 */

			if (value == null)
			{
				writer.WriteNullValue();
			}
			else
			{
				var multipleDays = string.Join(",", value.Select(day => (Convert.ToInt32(day) + 1).ToString()));
				writer.WriteStringValue(multipleDays);
			}
		}
	}
}
