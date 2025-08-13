using System;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="Interpreter"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class InterpreterConverter : ZoomNetJsonConverter<Interpreter>
	{
		public override Interpreter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var interpreterType = rootElement.GetPropertyValue<InterpreterType>("type");

			switch (interpreterType)
			{
				case InterpreterType.Sign:
					return rootElement.ToObject<SignLanguageInterpreter>(options);
				case InterpreterType.Language:
					return rootElement.ToObject<LanguageInterpreter>(options);
				default:
					throw new JsonException($"{interpreterType} is an unknown type of interpreter");
			}
		}
	}
}
