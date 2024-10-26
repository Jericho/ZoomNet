using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="ParticipantDevice"/> to or from JSON.
	/// </summary>
	/// <seealso cref="JsonConverter" />
	internal class ParticipantDeviceConverter : ZoomNetJsonConverter<ParticipantDevice[]>
	{
		public override ParticipantDevice[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
					return Array.Empty<ParticipantDevice>();
				case JsonTokenType.String:
					var stringValue = reader.GetString();
					var items = stringValue
						.Split(new[] { '+' })
						.Where(item => !long.TryParse(item, out _)) // Filter out values like "17763" which is a Windows build number. See https://github.com/Jericho/ZoomNet/issues/354 for details
						.Select(item => Convert(item))
						.ToArray();

					return items;
				default:
					throw new JsonException("Unable to convert to ParticipantDevice");
			}
		}

		public override void Write(Utf8JsonWriter writer, ParticipantDevice[] value, JsonSerializerOptions options)
		{
			var stringValue = string.Join(" + ", value.Select(device => device.ToEnumString()));
			writer.WriteStringValue(stringValue);
		}

		private static ParticipantDevice Convert(string deviceAsString)
		{
			if (string.IsNullOrWhiteSpace(deviceAsString)) return ParticipantDevice.Unknown;

			// See https://github.com/Jericho/ZoomNet/issues/369 for details about the underlying problem
			// See https://github.com/Jericho/ZoomNet/issues/370 for details about this workaround
			if (deviceAsString.StartsWith("Web Browser", StringComparison.OrdinalIgnoreCase)) return ParticipantDevice.Web;

			return deviceAsString.Trim().ToEnum<ParticipantDevice>();
		}
	}
}
