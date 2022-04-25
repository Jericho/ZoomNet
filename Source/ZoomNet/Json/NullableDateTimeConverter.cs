using System;
using System.Globalization;
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
					return null;
				case JsonTokenType.String:
					var stringValue = reader.GetString();
					if (string.IsNullOrEmpty(stringValue)) return null;
					return DateTime.Parse(stringValue, null, DateTimeStyles.AdjustToUniversal);
				default:
					throw new Exception("Unable to convert to nullable DateTime");
			}
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			if (value.HasValue) writer.WriteStringValue(value.Value.ToZoomFormat());
			else writer.WriteNullValue();
		}
	}
}
