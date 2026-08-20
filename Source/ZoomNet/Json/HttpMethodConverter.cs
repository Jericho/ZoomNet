using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="HttpMethod">HTTP method (AKA verb)</see> to or from JSON.
	/// </summary>
	/// <seealso cref="JsonConverter" />
	internal class HttpMethodConverter : JsonConverter<HttpMethod>
	{
		public override HttpMethod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
					return null;

				case JsonTokenType.String:
					return new HttpMethod(reader.GetString());

				default:
					throw new JsonException($"Unable to convert the content of {reader.TokenType.ToEnumString()} JSON node into a HttpMethod value)");
			}
		}

		public override void Write(Utf8JsonWriter writer, HttpMethod value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Method);
		}
	}
}
