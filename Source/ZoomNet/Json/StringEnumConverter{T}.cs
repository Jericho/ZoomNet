using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZoomNet.Utilities;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an <see cref="Enum"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class StringEnumConverter<T> : ZoomNetJsonConverter<T>
		where T : Enum
	{
		// 'Preference' is used to determine which string value to use when serializing an enum value. The lower the number, the higher the preference.
		private static readonly Dictionary<T, (List<(string Value, int Preference)> Strings, int IntValue)> _enumToString = new();

		static StringEnumConverter()
		{
			var enumType = typeof(T);

			foreach (var name in Enum.GetNames(enumType))
			{
				var customAttributes = enumType.GetField(name).GetCustomAttributes(true);
				var enumMemberAttribute = customAttributes.OfType<EnumMemberAttribute>().SingleOrDefault();
				var jsonPropertyNameAttribute = customAttributes.OfType<JsonPropertyNameAttribute>().SingleOrDefault();
				var descriptionAttribute = customAttributes.OfType<DescriptionAttribute>().SingleOrDefault();
				var multipleValuesEnumMemberAttribute = customAttributes.OfType<MultipleValuesEnumMemberAttribute>().SingleOrDefault();

				var enumValue = (T)Enum.Parse(enumType, name);

				// Add enum name but set its 'Preference' to int.MaxValue. This ensures that the enum name can be used for deserialization but not for serialization.
				//
				// THIS IS A VERY DELIBERATE CHOICE!
				// When a given enum value is not decorated with any of the attributes, we want to use the integer value for serialization. This is mandated by the Zoom API.
				//
				// The fallback logic in the Write method of this converter uses the integer value of the enum which is the value expected by the Zoom API.
				// However, please note that the fallback value in the ToEnumString extension method is the enum name. I did not change this logic because I wanted to maintain backward
				// compatibility even though I believe this fallback logic should be changed to match the logic in the Write method of this converter. changing the fallback logic
				// would break a few unit tests that are currently passing. I will leave this as is for now but I may change it in the future.
				_enumToString.Add(enumValue, (new List<(string Value, int Preference)> { (name, int.MaxValue) }, System.Convert.ToInt32(enumValue)));

				// Old logic favored MultipleValuesEnumMember default over EnumMember/JsonPropertyName/Description when serializing.
				// To keep backward compatibility, set preferences accordingly:
				// 1 = MultipleValues.Default,
				// 2 = EnumMember,
				// 3 = JsonPropertyName,
				// 4 = Description
				if (multipleValuesEnumMemberAttribute is not null)
				{
					// Only the DefaultValue should be used for serialization; OtherValues are for parsing only.
					_enumToString[enumValue].Strings.Add((multipleValuesEnumMemberAttribute.DefaultValue, 1));

					// Add other values but set their preference to int.MaxValue so they are not chosen for serialization
					_enumToString[enumValue].Strings.AddRange((multipleValuesEnumMemberAttribute.OtherValues ?? System.Array.Empty<string>()).Select(v => (v, int.MaxValue)));
				}

				if (enumMemberAttribute is not null) _enumToString[enumValue].Strings.Add((enumMemberAttribute.Value, 2));
				if (jsonPropertyNameAttribute is not null) _enumToString[enumValue].Strings.Add((jsonPropertyNameAttribute.Name, 3));
				if (descriptionAttribute is not null) _enumToString[enumValue].Strings.Add((descriptionAttribute.Description, 4));
			}
		}

		public static T Convert(string stringValue)
		{
			if (TryConvert(stringValue, out T enumValue)) return enumValue;
			else throw new ArgumentException($"There is no value in the {typeof(T).Name} enum that corresponds to '{stringValue}'.");
		}

		public static bool TryConvert(string stringValue, out T value)
		{
			var strings = _enumToString.Where(kvp => kvp.Value.Strings.Any(s => string.Equals(s.Value, stringValue ?? string.Empty, StringComparison.OrdinalIgnoreCase)));
			if (strings.Any())
			{
				value = strings.First().Key;
				return true;
			}

			// In rare scenarios the Zoom API returns the numerical value as a string.
			// For example: an integer value like 1 is returned as the string "1".
			if (int.TryParse(stringValue, out int numberValue))
			{
				value = (T)Enum.ToObject(typeof(T), numberValue);
				return true;
			}

			value = default;
			return false;
		}

		public static bool TryConvert(T enumValue, out string stringValue, bool throwWhenUndefined = true)
		{
			if (_enumToString.TryGetValue(enumValue, out var strings))
			{
				var stringValues = strings.Strings.Where(s => s.Preference != int.MaxValue);
				if (stringValues.Any())
				{
					stringValue = stringValues.OrderBy(s => s.Preference).First().Value;
					return true;
				}
			}

			if (throwWhenUndefined)
			{
				throw new ArgumentException($"{enumValue} is not a valid value for {typeof(T).Name}", nameof(enumValue));
			}

			stringValue = null;
			return false;
		}

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.String:
					var stringValue = reader.GetString();
					if (StringEnumConverter<T>.TryConvert(stringValue, out T enumValue)) return enumValue;
					else throw new JsonException($"There is no value in the {typeof(T).Name} enum that corresponds to '{stringValue}'.");

				case JsonTokenType.Number:
					var numberValue = reader.GetInt32();
					return (T)Enum.ToObject(typeof(T), numberValue);

				case JsonTokenType.Null:
					if (TryConvert(string.Empty, out T value)) return value;
					throw new JsonException($"Unable to convert a null value into a {typeToConvert?.Name ?? typeof(T).Name} enum.");

				default:
					throw new JsonException($"Unexpected token {reader.TokenType} when parsing an enum.");
			}
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			if (value is null)
			{
				writer.WriteNullValue();
			}
			else if (value.TryToEnumString(out var stringValue, false))
			{
				writer.WriteStringValue(stringValue);
			}
			else
			{
				writer.WriteNumberValue(System.Convert.ToInt32(value));
			}
		}
	}
}
