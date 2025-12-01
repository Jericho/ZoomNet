using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class DateOnlyTimeConverterTests
	{
		[Fact]
		public void Read()
		{
			// Arrange
			var json = "\"2025-09-17T09:18:33\"";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new DateTimeConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new DateTime(2025, 9, 17, 9, 18, 33));
		}

		[Fact]
		public void Throws_when_null_value()
		{
			Action lambda = () =>
			{
				// Arrange
				var json = "null";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new DateTimeConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to convert a null value to DateTime");
		}

		[Fact]
		public void Throws_when_empty_string()
		{
			Action lambda = () =>
			{
				// Arrange
				var json = "\"\"";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new DateTimeConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to convert a null value to DateTime");
		}

		[Fact]
		public void Throws_when_reading_any_other_data_type()
		{
			Action lambda = () =>
			{
				// Arrange
				var json = "2025";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new DateTimeConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to convert Number to DateTime");
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = new DateTime(2025, 9, 17, 9, 20, 44, DateTimeKind.Utc);
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new DateTimeConverter();

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
