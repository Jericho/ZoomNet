using System;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="Nullable{DateTime}">nullable DateTime</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class NullableDateTimeConverter : ZoomNetJsonConverter<DateTime?>
	{
		private readonly DateTimeConverter _dateTimeConverter = new DateTimeConverter();

		public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
				case JsonTokenType.String when string.IsNullOrEmpty(reader.GetString()):
					return null;

				default:
					return _dateTimeConverter.Read(ref reader, typeToConvert, options);
			}
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			if (!value.HasValue) writer.WriteNullValue();
			else _dateTimeConverter.Write(writer, value.Value, options);
		}
	}
}
