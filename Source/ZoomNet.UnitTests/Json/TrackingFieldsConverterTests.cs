using Shouldly;
using System;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class TrackingFieldsConverterTests
	{
		[Fact]
		public void Read()
		{
			// Arrange
			var json = @"[
				{ ""field"": ""some_field"", ""value"": ""ABC_123"" },
				{ ""field"": ""another_field"", ""value"": ""FooBar"" }
			]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TrackingFieldsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Key.ShouldBe("some_field");
			result[0].Value.ShouldBe("ABC_123");
			result[1].Key.ShouldBe("another_field");
			result[1].Value.ShouldBe("FooBar");
		}
	}
}
