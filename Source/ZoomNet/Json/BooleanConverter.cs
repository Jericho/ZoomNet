using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="bool">boolean</see> to or from JSON.
	/// </summary>
	/// <seealso cref="JsonConverter" />
	internal class BooleanConverter : JsonConverter<bool>
	{
		public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.True:
				case JsonTokenType.False:
					return reader.GetBoolean();
				case JsonTokenType.Number:
					return reader.GetByte() == 1;
				default:
					throw new JsonException($"Unable to convert the content of {reader.TokenType.ToEnumString()} JSON node into a boolean value");
			}
		}

		public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
		{
			writer.WriteBooleanValue(value);
		}
	}
}
