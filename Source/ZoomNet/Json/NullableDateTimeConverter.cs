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
		public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
				case JsonTokenType.String when string.IsNullOrEmpty(reader.GetString()):
					return null;
				case JsonTokenType.String:
					return reader.GetDateTime();
				default:
					throw new Exception($"Unable to convert {reader.TokenType.ToEnumString()} to nullable DateTime");
			}
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			if (value.HasValue) writer.WriteStringValue(value.Value.ToZoomFormat());
			else writer.WriteNullValue();
		}
	}
}
