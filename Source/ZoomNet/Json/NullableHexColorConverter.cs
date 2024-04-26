using System;
using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json;

/// <summary>
/// Converts a <see cref="Nullable"/>&lt;<see cref="Color"/>&gt; to or from JSON in Hex format.
/// </summary>
public class NullableHexColorConverter : JsonConverter<Color?>
{
	/// <inheritdoc/>
	public override bool CanConvert(Type typeToConvert)
	{
		return typeToConvert == typeof(Color?);
	}

	/// <inheritdoc/>
	public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
			case JsonTokenType.None:
			case JsonTokenType.Null:
			case JsonTokenType.String when string.IsNullOrEmpty(reader.GetString()):
				return null;

			default:
				{
					var str = reader.GetString().Replace("#", string.Empty);
					if (str.Length == 6) str = $"FF{str}";
					return int.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var argB)
						? Color.FromArgb(argB)
						: null;
				}
		}
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
	{
		if (!value.HasValue) writer.WriteNullValue();
		else writer.WriteStringValue($"#{value.Value.R:X2}{value.Value.G:X2}{value.Value.B:X2}");
	}
}
