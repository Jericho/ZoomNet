using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class DelimitedStringConverterTests
	{
		[Fact]
		public void Returns_empty_aray_when_null_value()
		{
			// Arrange
			var json = "null";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DelimitedStringConverter();

			// Act
			jsonReader.Read();

			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBeEmpty();
		}

		[Theory]
		[InlineData("\"1,2\"", ",", new[] { "1", "2" })]
		[InlineData("\"aaa;bbb\"", ";", new[] { "aaa", "bbb" })]
		[InlineData("\"1,,2\"", ",", new[] { "1", "2" })] // <-- notice the empty entry
		public void Read(string value, string delimiter, string[] expected)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DelimitedStringConverter(delimiter);

			// Act
			jsonReader.Read();

			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData(new[] { "aaa", "bbb" }, ";", "\"aaa;bbb\"")]
		[InlineData((string[])null, ";", "null")]
		public void Write(string[] value, string delimiter, string expected)
		{
			// Arrange
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new DelimitedStringConverter(delimiter);

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe(expected);
		}
	}
}
