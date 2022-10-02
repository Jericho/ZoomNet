using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an <see cref="Enum"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class StringEnumConverter<T> : ZoomNetJsonConverter<T>
		where T : Enum
	{
		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.String:
					var stringValue = reader.GetString();
					return stringValue.ToEnum<T>();

				case JsonTokenType.Number:
					var numberValue = reader.GetInt32();
					return (T)Enum.ToObject(typeof(T), numberValue);

				case JsonTokenType.Null:
					var enumType = typeof(T);
					if (Nullable.GetUnderlyingType(enumType) != null) return default;

					foreach (var name in Enum.GetNames(enumType))
					{
						// Get the custom attributes on the enum item
						var customAttributes = enumType.GetField(name).GetCustomAttributes(true);

						// See if there's an 'EnumMember' attribute with an empty value
						if (customAttributes.OfType<EnumMemberAttribute>().Any(attribute => string.IsNullOrEmpty(attribute.Value))) return (T)Enum.Parse(enumType, name);

						// See if there's an 'JsonPropertyNameAttribute' attribute with an empty value
						if (customAttributes.OfType<JsonPropertyNameAttribute>().Any(attribute => string.IsNullOrEmpty(attribute.Name))) return (T)Enum.Parse(enumType, name);

						// See if there's a 'Description' attribute with an empty value
						if (customAttributes.OfType<DescriptionAttribute>().Any(attribute => string.IsNullOrEmpty(attribute.Description))) return (T)Enum.Parse(enumType, name);
					}

					throw new JsonException($"Unable to convert a null value into a {typeToConvert.Name} enum.");

				default:
					throw new JsonException($"Unexpected token {reader.TokenType} when parsing an enum.");
			}
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNullValue();
			}
			else if (value.TryToEnumString(out var stringValue))
			{
				writer.WriteStringValue(stringValue);
			}
			else
			{
				writer.WriteNumberValue(Convert.ToInt32(value));
			}
		}
	}
}
