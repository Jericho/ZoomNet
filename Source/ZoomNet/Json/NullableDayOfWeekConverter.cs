using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a nullable <see cref="DayOfWeek">day of the week</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class NullableDayOfWeekConverter : JsonConverter<DayOfWeek?>
	{
		private readonly DayOfWeekConverter _dayOfWeekConverter = new DayOfWeekConverter();

		public override DayOfWeek? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
					return null;

				default:
					return _dayOfWeekConverter.Read(ref reader, typeToConvert, options);
			}
		}

		public override void Write(Utf8JsonWriter writer, DayOfWeek? value, JsonSerializerOptions options)
		{
			if (!value.HasValue) writer.WriteNullValue();
			else _dayOfWeekConverter.Write(writer, value.Value, options);
		}
	}
}
