using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="KeyValuePair{String, String}"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class KeyValuePairConverter : ZoomNetJsonConverter<KeyValuePair<string, string>[]>
	{
		private const string DefaultKeyFieldName = "key";
		private const string DefaultValueFieldName = "value";

		private readonly string _keyFieldName;
		private readonly string _valueFieldName;

		public KeyValuePairConverter()
			: this(DefaultKeyFieldName, DefaultValueFieldName)
		{
		}

		public KeyValuePairConverter(string keyFieldName, string valueFieldName)
		{
			_keyFieldName = keyFieldName;
			_valueFieldName = valueFieldName;
		}

		public override KeyValuePair<string, string>[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.StartArray)
			{
				var values = new List<KeyValuePair<string, string>>();

				while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				{
					if (reader.TokenType == JsonTokenType.StartObject)
					{
						var fieldName = string.Empty;
						var fieldValue = string.Empty;

						while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
						{
							if (reader.TokenType == JsonTokenType.PropertyName)
							{
								var propertyName = reader.GetString();
								reader.Read();

								if (propertyName == _keyFieldName) fieldName = reader.GetString();
								else if (propertyName == _valueFieldName) fieldValue = reader.GetString();
							}
						}

						if (!string.IsNullOrEmpty(fieldName)) values.Add(new KeyValuePair<string, string>(fieldName, fieldValue));
					}
				}

				return values.ToArray();
			}

			throw new Exception("Unable to read Key/Value pair");
		}

		public override void Write(Utf8JsonWriter writer, KeyValuePair<string, string>[] value, JsonSerializerOptions options)
		{
			if (value == null) return;

			writer.WriteStartArray();

			foreach (var item in value)
			{
				writer.WriteStartObject();
				writer.WriteString(_keyFieldName, item.Key);
				writer.WriteString(_valueFieldName, item.Value);
				writer.WriteEndObject();
			}

			writer.WriteEndArray();
		}
	}
}
