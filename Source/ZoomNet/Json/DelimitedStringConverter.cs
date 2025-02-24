using System;
using System.Linq;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a delimited string to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class DelimitedStringConverter : ZoomNetJsonConverter<string[]>
	{
		private const string DefaultDelimiter = ",";

		private readonly string _delimiter;

		public DelimitedStringConverter()
			: this(DefaultDelimiter)
		{
		}

		public DelimitedStringConverter(string delimiter)
		{
			_delimiter = delimiter;
		}

		public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null) return Array.Empty<string>();

			var rawValue = reader.GetString();
			return rawValue
				.Split(new[] { _delimiter }, StringSplitOptions.RemoveEmptyEntries)
				.ToArray();
		}

		public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
		{
			if (value == null) writer.WriteNullValue();
			else writer.WriteStringValue(string.Join(_delimiter, value));
		}
	}
}
