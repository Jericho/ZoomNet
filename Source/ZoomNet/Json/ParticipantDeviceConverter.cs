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
						.Select(item => Convert(item))
						.ToArray();

					return items;
				default:
					throw new Exception("Unable to convert to ParticipantDevice");
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
			return deviceAsString.Trim().ToEnum<ParticipantDevice>();
		}
	}
}
