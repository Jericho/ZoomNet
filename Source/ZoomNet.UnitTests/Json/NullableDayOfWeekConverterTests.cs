using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class NullableDayOfWeekConverterTests
	{
		[Theory]
		[InlineData("null")] // Null value
		public void Read_null(string value)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new NullableDayOfWeekConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBeNull();
		}

		[Fact]
		public void Read()
		{
			// Arrange
			var value = 7;
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
			result.ShouldBe(DayOfWeek.Saturday);
		}

		[Fact]
		public void Write_null()
		{
			// Arrange
			var value = (DayOfWeek?)null;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new NullableDayOfWeekConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("null");
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = DayOfWeek.Saturday;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new NullableDayOfWeekConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("7");
		}
	}
}
