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
		private readonly KeyValuePairConverter _converter = new KeyValuePairConverter("field", "value");

		public override KeyValuePair<string, string>[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return _converter.Read(ref reader, typeToConvert, options);
		}

		public override void Write(Utf8JsonWriter writer, KeyValuePair<string, string>[] value, JsonSerializerOptions options)
		{
			_converter.Write(writer, value, options);
		}
	}
}
