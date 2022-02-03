using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Converts a JSON string into and array of devices.
	/// </summary>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	internal class ParticipantDeviceConverter : JsonConverter
	{
		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>
		/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(string);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.
		/// </value>
		public override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.
		/// </value>
		public override bool CanWrite
		{
			get { return true; }
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var stringValue = string.Join(" + ", ((IEnumerable<ParticipantDevice>)value).Select(device => device.ToEnumString()));
			serializer.Serialize(writer, stringValue);
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The object value.
		/// </returns>
		/// <exception cref="System.Exception">Unable to determine the field type.</exception>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String)
			{
				var stringValue = (string)reader.Value;
				var items = stringValue
					.Split(new[] { "+" }, StringSplitOptions.RemoveEmptyEntries)
					.Select(item => Convert(item))
					.ToArray();

				return items;
			}

			throw new Exception("Unable to convert to ParticipantDevice");
		}

		private static ParticipantDevice Convert(string deviceAsString)
		{
			if (string.IsNullOrWhiteSpace(deviceAsString)) return ParticipantDevice.Unknown;
			return deviceAsString.Trim().ToEnum<ParticipantDevice>();
		}
	}
}
