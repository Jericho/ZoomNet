using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class DaysOfWeekConverterTests
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

			var converter = new DaysOfWeekConverter();

			// Act
			jsonReader.Read();

			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBeEmpty();
		}

		[Theory]
		[InlineData("\"1\"", new[] { DayOfWeek.Sunday })]
		[InlineData("\"2,3\"", new[] { DayOfWeek.Monday, DayOfWeek.Tuesday })]
		[InlineData("[2,3]", new[] { DayOfWeek.Monday, DayOfWeek.Tuesday })]
		public void Read(string value, DayOfWeek[] expected)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DaysOfWeekConverter();

			// Act
			jsonReader.Read();

			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expected);
		}

		[Fact]
		public void Throws_when_other_data_type()
		{
			// Arrange
			var value = 12;
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value.ToString());
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DaysOfWeekConverter();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe($"Unexpected token type: Number. Unable to convert to Array of DayOfWeek.");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}

		[Theory]
		[InlineData((DayOfWeek[])null, true, "null")]
		[InlineData((DayOfWeek[])null, false, "null")]
		[InlineData(new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }, true, "\"7,1\"")]
		[InlineData(new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }, false, "[7,1]")]
		public void Write(DayOfWeek[] value, bool serializeAsCommaDelimitedString, string expected)
		{
			// Arrange
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new DaysOfWeekConverter(serializeAsCommaDelimitedString);

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
