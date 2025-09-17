using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class EpochConverterTests
	{
		[Fact]
		public void Read()
		{
			// Arrange
			var json = "1758100713";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new EpochConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new DateTime(2025, 9, 17, 9, 18, 33));
		}

		[Theory]
		[InlineData("null", "Null")]
		[InlineData("\"abc\"", "String")]
		[InlineData("\"\"", "String")]
		public void Throws_when_invalid_value(string value, string typeName)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new EpochConverter();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe($"Unable to convert {typeName} from Epoch to DateTime");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = new DateTime(2025, 9, 17, 9, 18, 33, DateTimeKind.Utc);
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new EpochConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("1758100713");
		}
	}
}
