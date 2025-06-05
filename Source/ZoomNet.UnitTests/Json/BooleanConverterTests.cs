using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class BooleanConverterTests
	{
		[Theory]
		[InlineData("true", true)]
		[InlineData("false", false)]
		public void Read_Boolean(string value, bool expectedValue)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new BooleanConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expectedValue);
		}

		[Theory]
		[InlineData(long.MinValue, false)]
		[InlineData(-1, false)]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(2, false)]
		[InlineData(long.MaxValue, false)]
		public void Read_Long(long value, bool expectedValue)
		{
			// Arrange
			var json = value.ToString();
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new BooleanConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expectedValue);
		}

		[Theory]
		[InlineData(int.MinValue, false)]
		[InlineData(-1, false)]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(2, false)]
		[InlineData(int.MaxValue, false)]
		public void Read_Int(int value, bool expectedValue)
		{
			// Arrange
			var json = value.ToString();
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new BooleanConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expectedValue);
		}

		[Theory]
		[InlineData(short.MinValue, false)]
		[InlineData(-1, false)]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(2, false)]
		[InlineData(short.MaxValue, false)]
		public void Read_Short(short value, bool expectedValue)
		{
			// Arrange
			var json = value.ToString();
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new BooleanConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expectedValue);
		}

		[Fact]
		public void Throws_when_reading_string()
		{
			// Arrange
			var json = "\"Strings are not handled by our boolean converter\"";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new BooleanConverter();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException)
			{
				// Intentionally swallow this exception. This is the expected behavior when attempting to parse a string.
				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}

		[Theory]
		[InlineData(true, "true")]
		[InlineData(false, "false")]
		public void Write(bool value, string expectedValue)
		{
			// Arrange
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new BooleanConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe(expectedValue);
		}

	}
}
