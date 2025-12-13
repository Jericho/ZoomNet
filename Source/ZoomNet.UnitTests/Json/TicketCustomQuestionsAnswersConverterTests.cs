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
	public class TicketCustomQuestionsAnswersConverterTests
	{
		#region Read Method Tests

		[Fact]
		public void Read_ValidArray_DeserializesCorrectly()
		{
			// Arrange
			var json = "[{\"title\": \"Question 1\", \"answer\": \"Answer 1\"},{\"title\": \"Question 2\", \"answer\": \"Answer 2\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Key.ShouldBe("Question 1");
			result[0].Value.ShouldBe("Answer 1");
			result[1].Key.ShouldBe("Question 2");
			result[1].Value.ShouldBe("Answer 2");
		}

		[Fact]
		public void Read_EmptyArray_ReturnsEmptyArray()
		{
			// Arrange
			var json = "[]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public void Read_ArrayWithSingleItem_DeserializesCorrectly()
		{
			// Arrange
			var json = "[{\"title\": \"Single Question\", \"answer\": \"Single Answer\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Key.ShouldBe("Single Question");
			result[0].Value.ShouldBe("Single Answer");
		}

		[Fact]
		public void Read_ArrayWithEmptyStringValues_DeserializesCorrectly()
		{
			// Arrange
			var json = "[{\"title\": \"Question\", \"answer\": \"\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Key.ShouldBe("Question");
			result[0].Value.ShouldBe("");
		}

		[Fact]
		public void Read_ArrayWithSpecialCharacters_DeserializesCorrectly()
		{
			// Arrange
			var json = "[{\"title\": \"What is your name?\", \"answer\": \"John \\\"Doe\\\" Smith\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Key.ShouldBe("What is your name?");
			result[0].Value.ShouldBe("John \"Doe\" Smith");
		}

		[Fact]
		public void Read_ArrayWithMalformedItems_IgnoresMalformedItems()
		{
			// Arrange - First item uses "key" instead of "title", second item is valid
			var json = "[{\"key\": \"Wrong Field\", \"answer\": \"Should be ignored\"},{\"title\": \"Correct Question\", \"answer\": \"Correct Answer\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Key.ShouldBe("Correct Question");
			result[0].Value.ShouldBe("Correct Answer");
		}

		[Fact]
		public void Read_ArrayWithMissingAnswerField_IncludesItemWithEmptyAnswer()
		{
			// Arrange - Item has title but no answer
			var json = "[{\"title\": \"Question without answer\"},{\"title\": \"Complete Question\", \"answer\": \"Complete Answer\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Key.ShouldBe("Question without answer");
			result[0].Value.ShouldBe("");
			result[1].Key.ShouldBe("Complete Question");
			result[1].Value.ShouldBe("Complete Answer");
		}

		[Fact]
		public void Read_ArrayWithEmptyTitleField_IgnoresItem()
		{
			// Arrange - Item has empty title
			var json = "[{\"title\": \"\", \"answer\": \"Answer without question\"},{\"title\": \"Valid Question\", \"answer\": \"Valid Answer\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Key.ShouldBe("Valid Question");
			result[0].Value.ShouldBe("Valid Answer");
		}

		[Fact]
		public void Read_NonArrayValue_ThrowsJsonException()
		{
			// Arrange
			Action lambda = () =>
			{
				var json = "\"This is a string, not an array\"";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new TicketCustomQuestionsAnswersConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to read Key/Value pair");
		}

		[Fact]
		public void Read_ObjectInsteadOfArray_ThrowsJsonException()
		{
			// Arrange
			Action lambda = () =>
			{
				var json = "{\"title\": \"Question\", \"answer\": \"Answer\"}";
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = new JsonSerializerOptions();

				var converter = new TicketCustomQuestionsAnswersConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to read Key/Value pair");
		}

		[Fact]
		public void Read_ArrayWithMultipleQuestionsAndAnswers_PreservesOrder()
		{
			// Arrange
			var json = "[" +
				"{\"title\": \"Q1\", \"answer\": \"A1\"}," +
				"{\"title\": \"Q2\", \"answer\": \"A2\"}," +
				"{\"title\": \"Q3\", \"answer\": \"A3\"}," +
				"{\"title\": \"Q4\", \"answer\": \"A4\"}" +
				"]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(4);
			for (int i = 0; i < 4; i++)
			{
				result[i].Key.ShouldBe($"Q{i + 1}");
				result[i].Value.ShouldBe($"A{i + 1}");
			}
		}

		#endregion

		#region Write Method Tests

		[Fact]
		public void Write_ValidArray_SerializesCorrectly()
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

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"title\":\"Question 1\",\"answer\":\"Answer 1\"},{\"title\":\"Question 2\",\"answer\":\"Answer 2\"}]");
		}

		[Fact]
		public void Write_EmptyArray_SerializesAsEmptyArray()
		{
			// Arrange
			var value = new KeyValuePair<string, string>[0];
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[]");
		}

		[Fact]
		public void Write_NullArray_WritesNothing()
		{
			// Arrange
			KeyValuePair<string, string>[] value = null;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("");
		}

		[Fact]
		public void Write_SingleItem_SerializesCorrectly()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("Single Question", "Single Answer")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"title\":\"Single Question\",\"answer\":\"Single Answer\"}]");
		}

		[Fact]
		public void Write_ItemsWithEmptyValues_SerializesCorrectly()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("Question", ""),
				new KeyValuePair<string, string>("", "Answer")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"title\":\"Question\",\"answer\":\"\"},{\"title\":\"\",\"answer\":\"Answer\"}]");
		}

		[Fact]
		public void Write_ItemsWithSpecialCharacters_EscapesCorrectly()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("What is your \"name\"?", "John \"Doe\" Smith")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldContain("\\u0022");
			result.ShouldContain("What is your \\u0022name\\u0022?");
			result.ShouldContain("John \\u0022Doe\\u0022 Smith");
		}

		[Fact]
		public void Write_MultipleItems_PreservesOrder()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("Q1", "A1"),
				new KeyValuePair<string, string>("Q2", "A2"),
				new KeyValuePair<string, string>("Q3", "A3"),
				new KeyValuePair<string, string>("Q4", "A4")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"title\":\"Q1\",\"answer\":\"A1\"},{\"title\":\"Q2\",\"answer\":\"A2\"},{\"title\":\"Q3\",\"answer\":\"A3\"},{\"title\":\"Q4\",\"answer\":\"A4\"}]");
		}

		#endregion

		#region RoundTrip Tests

		[Fact]
		public void RoundTrip_SimpleArray_PreservesData()
		{
			// Arrange
			var original = new[]
			{
				new KeyValuePair<string, string>("Question 1", "Answer 1"),
				new KeyValuePair<string, string>("Question 2", "Answer 2"),
				new KeyValuePair<string, string>("Question 3", "Answer 3")
			};

			var converter = new TicketCustomQuestionsAnswersConverter();
			var options = new JsonSerializerOptions();

			// Act - Serialize
			var ms = new MemoryStream();
			using (var writer = new Utf8JsonWriter(ms))
			{
				converter.Write(writer, original, options);
			}

			var json = Encoding.UTF8.GetString(ms.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserialized = converter.Read(ref jsonReader, typeof(KeyValuePair<string, string>[]), options);

			// Assert
			deserialized.ShouldNotBeNull();
			deserialized.Length.ShouldBe(3);
			for (int i = 0; i < 3; i++)
			{
				deserialized[i].Key.ShouldBe(original[i].Key);
				deserialized[i].Value.ShouldBe(original[i].Value);
			}
		}

		[Fact]
		public void RoundTrip_EmptyArray_PreservesEmptyArray()
		{
			// Arrange
			var original = new KeyValuePair<string, string>[0];

			var converter = new TicketCustomQuestionsAnswersConverter();
			var options = new JsonSerializerOptions();

			// Act - Serialize
			var ms = new MemoryStream();
			using (var writer = new Utf8JsonWriter(ms))
			{
				converter.Write(writer, original, options);
			}

			var json = Encoding.UTF8.GetString(ms.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserialized = converter.Read(ref jsonReader, typeof(KeyValuePair<string, string>[]), options);

			// Assert
			deserialized.ShouldNotBeNull();
			deserialized.Length.ShouldBe(0);
		}

		[Fact]
		public void RoundTrip_ArrayWithSpecialCharacters_PreservesAllCharacters()
		{
			// Arrange
			var original = new[]
			{
				new KeyValuePair<string, string>("Question with \"quotes\"", "Answer with \nnewlines"),
				new KeyValuePair<string, string>("Unicode: こんにちは", "Emoji: 😀")
			};

			var converter = new TicketCustomQuestionsAnswersConverter();
			var options = new JsonSerializerOptions();

			// Act - Serialize
			var ms = new MemoryStream();
			using (var writer = new Utf8JsonWriter(ms))
			{
				converter.Write(writer, original, options);
			}

			var json = Encoding.UTF8.GetString(ms.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserialized = converter.Read(ref jsonReader, typeof(KeyValuePair<string, string>[]), options);

			// Assert
			deserialized.ShouldNotBeNull();
			deserialized.Length.ShouldBe(2);
			deserialized[0].Key.ShouldBe(original[0].Key);
			deserialized[0].Value.ShouldBe(original[0].Value);
			deserialized[1].Key.ShouldBe(original[1].Key);
			deserialized[1].Value.ShouldBe(original[1].Value);
		}

		#endregion

		#region Constructor Tests

		[Fact]
		public void Constructor_SetsCorrectFieldNames()
		{
			// Arrange & Act
			var converter = new TicketCustomQuestionsAnswersConverter();

			// Assert - Verify by testing serialization uses "title" and "answer"
			var value = new[]
			{
				new KeyValuePair<string, string>("Test Question", "Test Answer")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Should use "title" and "answer" field names, not "key" and "value"
			result.ShouldContain("\"title\":");
			result.ShouldContain("\"answer\":");
			result.ShouldNotContain("\"key\":");
			result.ShouldNotContain("\"value\":");
		}

		#endregion

		#region Edge Cases

		[Fact]
		public void Read_ArrayWithNullValues_HandlesGracefully()
		{
			// Arrange
			var json = "[{\"title\": null, \"answer\": null}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public void Read_ArrayWithExtraFields_IgnoresExtraFields()
		{
			// Arrange
			var json = "[{\"title\": \"Question\", \"answer\": \"Answer\", \"extra_field\": \"Extra\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Key.ShouldBe("Question");
			result[0].Value.ShouldBe("Answer");
		}

		[Fact]
		public void Write_ArrayWithLongStrings_SerializesCorrectly()
		{
			// Arrange
			var longQuestion = new string('Q', 1000);
			var longAnswer = new string('A', 1000);
			var value = new[]
			{
				new KeyValuePair<string, string>(longQuestion, longAnswer)
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new TicketCustomQuestionsAnswersConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldContain(longQuestion);
			result.ShouldContain(longAnswer);
		}

		#endregion
	}
}
