using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="DayOfWeek">day of the week</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class DayOfWeekConverter : JsonConverter<DayOfWeek?>
	{
		public override DayOfWeek? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			/*
			 * IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
			 * but the values returned by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			 */

			if (reader.TokenType == JsonTokenType.Null) return null;

			var rawValue = reader.GetString();
			var value = Convert.ToInt32(rawValue) - 1;
			return (DayOfWeek)value;
		}

		public override void Write(Utf8JsonWriter writer, DayOfWeek? value, JsonSerializerOptions options)
		{
			/*
			 * IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
			 * but the values expected by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			 */

			if (!value.HasValue)
			{
				writer.WriteNullValue();
			}
			else
			{
				var singleDay = (Convert.ToInt32(value.Value) + 1).ToString();
				writer.WriteStringValue(singleDay);
			}
		}
	}
}
