using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="KeyValuePair{TKey, TValue}"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class TrackingFieldsConverter : ZoomNetJsonConverter<KeyValuePair<string, string>[]>
	{
		public override KeyValuePair<string, string>[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.StartArray)
			{
				var values = new List<KeyValuePair<string, string>>();

				reader.Read();

				while ((reader.TokenType != JsonTokenType.EndArray) && reader.Read())
				{
					if (reader.TokenType == JsonTokenType.StartObject)
					{
						var fieldName = string.Empty;
						var fieldValue = string.Empty;

						while ((reader.TokenType != JsonTokenType.EndObject) && reader.Read())
						{
							if (reader.TokenType == JsonTokenType.PropertyName)
							{
								var propertyName = reader.GetString();
								reader.Read();

								if (propertyName == "field") fieldName = reader.GetString();
								else if (propertyName == "value") fieldValue = reader.GetString();
							}
						}

						values.Add(new KeyValuePair<string, string>(fieldName, fieldValue));
					}
				}

				return values.ToArray();
			}

			throw new Exception("Unable to convert to tracking fields");
		}
	}
}
