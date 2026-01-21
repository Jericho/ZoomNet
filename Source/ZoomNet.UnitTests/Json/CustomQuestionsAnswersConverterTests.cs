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
	public class CustomQuestionsAnswersConverterTests
	{
		[Fact]
		public void Read_Array()
		{
			// Arrange
			var value = "[{\"title\": \"Question 1\", \"value\": \"Answer 1\"},{\"title\": \"Question 2\", \"value\": \"Answer 2\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new CustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("Question 1", "Answer 1"),
				new KeyValuePair<string, string>("Question 2", "Answer 2")
			});
		}

		[Fact]
		public void Anything_other_than_array_throws_an_exception()
		{
			Action lambda = () =>
			{
				// Arrange
				var value = "\"hello world, this is a string\"";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new CustomQuestionsAnswersConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to read Key/Value pair");
		}

		[Fact]
		public void Malformed_items_are_ignored()
		{
			// Arrange
			var value = "[{\"key\": \"Question 1\", \"value\": \"Answer 1\"},{\"title\": \"Question 2\", \"value\": \"Answer 2\"}]"; // first item is malformed (should be "title" not "key")
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new CustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("Question 2", "Answer 2")
			});
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("Question 1", "Answer 1"),
				new KeyValuePair<string, string>("Question 2", "Answer 2")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new CustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"title\":\"Question 1\",\"value\":\"Answer 1\"},{\"title\":\"Question 2\",\"value\":\"Answer 2\"}]");
		}

		[Fact]
		public void Write_NullValue_WritesNothing()
		{
			// Arrange
			KeyValuePair<string, string>[] value = null;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new CustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe(string.Empty);
		}

		[Fact]
		public void Read_Empty_Array()
		{
			// Arrange
			var value = "[]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new CustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public void Read_WithSpecialCharacters()
		{
			// Arrange
			var value = "[{\"title\": \"What's your name?\", \"value\": \"John O'Brien\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new CustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("What's your name?", "John O'Brien")
			});
		}

		[Fact]
		public void RoundTrip_MultipleItems()
		{
			// Arrange
			var originalValue = new[]
			{
				new KeyValuePair<string, string>("Question 1", "Answer 1"),
				new KeyValuePair<string, string>("Question 2", "Answer 2"),
				new KeyValuePair<string, string>("Question 3", "Answer 3")
			};

			// Serialize
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var converter = new CustomQuestionsAnswersConverter();
			converter.Write(jsonWriter, originalValue, new JsonSerializerOptions());
			jsonWriter.Flush();

			// Deserialize
			ms.Position = 0;
			var sr = new StreamReader(ms);
			var json = sr.ReadToEnd();
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserializedValue = converter.Read(ref jsonReader, null, new JsonSerializerOptions());

			// Assert
			deserializedValue.ShouldBe(originalValue);
		}

		[Fact]
		public void RealWorld_RegistrationQuestions()
		{
			// Arrange - Typical registration questions from a Zoom webinar
			var json = "[" +
				"{\"title\":\"Company\",\"value\":\"Acme Corp\"}," +
				"{\"title\":\"Job Title\",\"value\":\"Software Engineer\"}," +
				"{\"title\":\"Industry\",\"value\":\"Technology\"}," +
				"{\"title\":\"Phone Number\",\"value\":\"+1-555-123-4567\"}" +
				"]";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var converter = new CustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, null, new JsonSerializerOptions());

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(4);
			result[0].Key.ShouldBe("Company");
			result[0].Value.ShouldBe("Acme Corp");
			result[1].Key.ShouldBe("Job Title");
			result[1].Value.ShouldBe("Software Engineer");
			result[2].Key.ShouldBe("Industry");
			result[2].Value.ShouldBe("Technology");
			result[3].Key.ShouldBe("Phone Number");
			result[3].Value.ShouldBe("+1-555-123-4567");
		}

		[Fact]
		public void Read_ItemsWithMissingValue_AreIncluded()
		{
			// Arrange
			var json = "[{\"title\": \"Question 1\"},{\"title\": \"Question 2\", \"value\": \"Answer 2\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var converter = new CustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, null, new JsonSerializerOptions());

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Key.ShouldBe("Question 1");
			result[0].Value.ShouldBe(string.Empty); // Missing value becomes empty string
			result[1].Key.ShouldBe("Question 2");
			result[1].Value.ShouldBe("Answer 2");
		}
	}
}
