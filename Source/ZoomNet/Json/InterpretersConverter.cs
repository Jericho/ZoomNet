using System;
using System.Collections.Generic;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="Interpreter"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class InterpretersConverter<T> : ZoomNetJsonConverter<T[]>
		where T : Interpreter
	{
		private readonly InterpreterConverter _valueConverter;

		public InterpretersConverter()
		{
			_valueConverter = new InterpreterConverter();
		}

		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsArray && typeToConvert.IsAssignableFrom(typeof(Interpreter[]));
		}

		public override T[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
			{
				throw new JsonException();
			}

			var interpreters = new List<T>();

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndArray)
				{
					return interpreters.ToArray();
				}

				var interpreter = (T)_valueConverter.Read(ref reader, typeof(T), options)!;
				interpreters.Add(interpreter);
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, T[] interpreters, JsonSerializerOptions options)
		{
			writer.WriteStartArray();

			foreach (var interpreter in interpreters)
			{
				_valueConverter.Write(writer, interpreter, options);
			}

			writer.WriteEndArray();
		}
	}
}
