using Shouldly;
using System;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Json
{
	public class InterpreterConverterTests
	{
		private const string LANGUAGE_INTERPRETER_JSON = @"{
			""type"": 1,
			""email"": ""abc.def@email.com"",
			""source_language_id"": ""US"",
			""source_language_display_name"": ""English"",
			""target_language_id"": ""JP"",
			""target_language_display_name"": ""Japanese""
		}";
		private const string SIGN_LANGUAGE_INTERPRETER_JSON = @"{
			""type"": 2,
			""email"": ""abc.def@email.com"",
			""target_language_id"": ""FSL"",
			""target_language_display_name"": ""French""
		}";
		private const string INVALID_INTERPRETER_JSON = "{\"type\": 3}";

		[Theory]
		[InlineData(LANGUAGE_INTERPRETER_JSON, typeof(LanguageInterpreter))]
		[InlineData(SIGN_LANGUAGE_INTERPRETER_JSON, typeof(SignLanguageInterpreter))]
		public void Read(string json, Type expectedType)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new InterpreterConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBeOfType(expectedType);
		}

		[Fact]
		public void Throws_when_type_is_invalid()
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(INVALID_INTERPRETER_JSON);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new InterpreterConverter();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe("3 is an unknown type of interpreter");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}
	}
}
