using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Utilities.Json
{
	internal class NullableDateTimeConverter : JsonConverter<DateTime?>
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

					return DateTime.Parse(stringValue);
				default:
					throw new Exception("Unable to convert to ParticipantDevice");
			}
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
