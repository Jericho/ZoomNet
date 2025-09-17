using Shouldly;
using System;
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
			var json = "{\"key\": null}";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DaysOfWeekConverter();

			// Act
			jsonReader.Read(); // StartObject
			jsonReader.Read(); // PropertyName (which in this example is "key")
			jsonReader.Read(); // the Null value

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

			var converter = new DayOfWeekConverter();

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

		/*
		[Theory]
		[InlineData(DayOfWeek.Sunday, "1")]
		[InlineData(DayOfWeek.Monday, "2")]
		[InlineData(DayOfWeek.Tuesday, "3")]
		[InlineData(DayOfWeek.Wednesday, "4")]
		[InlineData(DayOfWeek.Thursday, "5")]
		[InlineData(DayOfWeek.Friday, "6")]
		[InlineData(DayOfWeek.Saturday, "7")]
		public void Write(DayOfWeek value, string expected)
		{
			// Arrange
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new DayOfWeekConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe(expected);
		}
		*/
	}
}
