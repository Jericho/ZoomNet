using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of app permissions to or from JSON.
	/// </summary>
	/// <seealso cref="KeyValuePairConverter"/>
	internal class AppPermissionsConverter : ZoomNetJsonConverter<string[]>
	{
		public AppPermissionsConverter()
		{
		}

		public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.StartArray)
			{
				var values = new List<string>();

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

								if (propertyName == "name") fieldValue = reader.GetString();
							}
						}

						if (!string.IsNullOrEmpty(fieldValue)) values.Add(fieldValue);
					}
				}

				return values.ToArray();
			}

			throw new JsonException("Unable to read app permissions");
		}

		public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
		{
			if (value == null) return;

			writer.WriteStartArray();

			foreach (var item in value)
			{
				writer.WriteStartObject();
				writer.WriteString("name", item);
				writer.WriteEndObject();
			}

			writer.WriteEndArray();
		}
	}
}
