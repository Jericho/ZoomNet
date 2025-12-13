using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class DayOfWeekConverterTests
	{
		[Theory]
		[InlineData(1, DayOfWeek.Sunday)]
		[InlineData(2, DayOfWeek.Monday)]
		[InlineData(3, DayOfWeek.Tuesday)]
		[InlineData(4, DayOfWeek.Wednesday)]
		[InlineData(5, DayOfWeek.Thursday)]
		[InlineData(6, DayOfWeek.Friday)]
		[InlineData(7, DayOfWeek.Saturday)]
		public void Read_ValidNumber(int value, DayOfWeek expected)
		{
			// Arrange
			var json = value.ToString();
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DayOfWeekConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData("1", DayOfWeek.Sunday)]
		[InlineData("2", DayOfWeek.Monday)]
		[InlineData("3", DayOfWeek.Tuesday)]
		[InlineData("4", DayOfWeek.Wednesday)]
		[InlineData("5", DayOfWeek.Thursday)]
		[InlineData("6", DayOfWeek.Friday)]
		[InlineData("7", DayOfWeek.Saturday)]
		public void FromJsonValue_string(string value, DayOfWeek expected)
		{
			// Act
			var result = DayOfWeekConverter.FromJsonValue(value);

			// Assert
			result.ShouldBe(expected);
		}

		[Theory]
		[InlineData("Hello world")]
		[InlineData("")]
		[InlineData("0")]
		[InlineData("-1")]
		[InlineData("66")]
		[InlineData((string)null)]
		public void FromJsonValue_invalid_string(string value)
		{
			// Act
			Should.Throw<JsonException>(() => DayOfWeekConverter.FromJsonValue(value));
		}

		[Fact]
		public void Throws_when_not_a_number()
		{
			Action lambda = () =>
			{
				// Arrange
				var json = "\"Hello World\"";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new DayOfWeekConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to convert String to DayOfWeek");
		}

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

		[Theory]
		[InlineData(DayOfWeek.Sunday, 1)]
		[InlineData(DayOfWeek.Monday, 2)]
		[InlineData(DayOfWeek.Tuesday, 3)]
		[InlineData(DayOfWeek.Wednesday, 4)]
		[InlineData(DayOfWeek.Thursday, 5)]
		[InlineData(DayOfWeek.Friday, 6)]
		[InlineData(DayOfWeek.Saturday, 7)]
		public void ToJsonValue(DayOfWeek value, int expected)
		{
			// Act
			var result = DayOfWeekConverter.ToJsonValue(value);

			// Assert
			result.ShouldBe(expected);
		}
	}
}
