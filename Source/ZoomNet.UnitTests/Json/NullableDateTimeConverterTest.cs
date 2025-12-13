using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class NullableDateTimeConverterTests
	{
		[Theory]
		[InlineData("\"\"")] // Empty string
		[InlineData("null")] // Null value
		public void Read_null(string value)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new NullableDateTimeConverter();

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
			var json = "\"2025-09-17T09:18:33\"";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new NullableDateTimeConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new DateTime(2025, 9, 17, 9, 18, 33));
		}

		[Fact]
		public void Write_null()
		{
			// Arrange
			var value = (DateTime?)null;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new NullableDateTimeConverter();

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
			var value = new DateTime(2025, 9, 17, 9, 20, 44, DateTimeKind.Utc);
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new NullableDateTimeConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("\"2025-09-17T09:20:44Z\"");
		}

	}
}
