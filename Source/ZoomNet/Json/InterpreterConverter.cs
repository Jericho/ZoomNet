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
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert == typeof(Interpreter) || typeToConvert.IsSubclassOf(typeof(Interpreter));
		}

		public override Interpreter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			// Scenario #1: The JSON contains a "type" property that indicates the type of interpreter.
			// This applies to the response from the "Get User Settings" endpoints.
			if (rootElement.TryGetProperty("type", out var typeProperty))
			{
				var interpreterType = typeProperty.ToObject<InterpreterType>();
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

			// Scenario #2: The JSON contains a "interpreter_languages" property that indicates a language interpreter.
			// This applies to the response from the "Get Meeting" and "Get Webinar" endpoints.
			if (rootElement.TryGetProperty("interpreter_languages", out var languagesProperty))
			{
				var email = rootElement.GetPropertyValue<string>("email");
				var languages = languagesProperty.GetString().Split(',');

				var sourceLanguage = (InterpretationLanguageForEventSession)Enum.Parse(typeof(InterpretationLanguageForEventSession), languages[0]);
				var targetLanguage = (InterpretationLanguageForEventSession)Enum.Parse(typeof(InterpretationLanguageForEventSession), languages[1]);

				return new LanguageInterpreter
				{
					Email = email,
					SourceLanguage = sourceLanguage,
					SourceLanguageDisplayName = languages[0],
					TargetLanguage = targetLanguage,
					TargetLanguageDisplayName = languages[1],
					Type = InterpreterType.Language
				};
			}

			// Scenario #3: The JSON contains a "sign_language" property that indicates a sign language interpreter.
			// This applies to the response from the "Get Meeting" and "Get Webinar" endpoints.
			if (rootElement.TryGetProperty("sign_language", out var signLanguageProperty))
			{
				var email = rootElement.GetPropertyValue<string>("email");
				var signLanguagName = signLanguageProperty.GetString();
				var signLanguage = (InterpretationSignLanguage)Enum.Parse(typeof(InterpretationSignLanguage), signLanguagName);

				return new SignLanguageInterpreter
				{
					Email = email,
					TargetLanguage = signLanguage,
					TargetLanguageDisplayName = signLanguagName,
					Type = InterpreterType.Sign
				};
			}

			// If we reach this point, we don't know how to deserialize the JSON into an Interpreter object.
			throw new JsonException("Unable to deserialize from JSON to an interpreter.");
		}
	}
}
