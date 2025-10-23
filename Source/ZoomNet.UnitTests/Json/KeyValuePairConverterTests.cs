using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;

namespace ZoomNet.UnitTests.Json
{
	public class KeyValuePairConverterTests
	{
		[Fact]
		public void Read_Array()
		{
			// Arrange
			var value = "[{\"key\": \"key1\", \"value\": \"value1\"},{\"key\": \"key2\", \"value\": \"value2\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new KeyValuePairConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("key1", "value1"),
				new KeyValuePair<string, string>("key2", "value2")
			});
		}

		[Fact]
		public void Anything_other_than_array_throws_an_exception()
		{
			// Arrange
			var value = "\"hello world, this is a string\"";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new KeyValuePairConverter();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe("Unable to read Key/Value pair");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}

		[Fact]
		public void Malformed_items_are_ignored()
		{
			// Arrange
			var value = "[{\"title\": \"key1\", \"value\": \"value1\"},{\"key\": \"key2\", \"value\": \"value2\"}]"; // first item is malformed (should be "key" not "title")
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new KeyValuePairConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("key2", "value2")
			});
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("key1", "value1"),
				new KeyValuePair<string, string>("key2", "value2")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new KeyValuePairConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"key\":\"key1\",\"value\":\"value1\"},{\"key\":\"key2\",\"value\":\"value2\"}]");
		}
	}
}
