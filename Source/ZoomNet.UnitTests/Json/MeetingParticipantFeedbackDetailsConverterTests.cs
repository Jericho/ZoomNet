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
	public class MeetingParticipantFeedbackDetailsConverterTests
	{
		[Fact]
		public void Read_Array()
		{
			// Arrange
			var value = "[{\"id\": \"1\", \"name\": \"Audio Quality\"},{\"id\": \"2\", \"name\": \"Video Quality\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("1", "Audio Quality"),
				new KeyValuePair<string, string>("2", "Video Quality")
			});
		}

		[Fact]
		public void Read_EmptyArray()
		{
			// Arrange
			var value = "[]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
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

				var converter = new MeetingParticipantFeedbackDetailsConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("Unable to read Key/Value pair");
		}

		[Fact]
		public void Malformed_items_with_wrong_field_names_are_ignored()
		{
			// Arrange
			// First item uses "key" instead of "id", third item uses "identifier" instead of "id"
			var value = "[{\"key\": \"1\", \"name\": \"Item 1\"},{\"id\": \"2\", \"name\": \"Item 2\"},{\"identifier\": \"3\", \"name\": \"Item 3\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("2", "Item 2")
			});
		}

		[Fact]
		public void Items_with_missing_id_are_ignored()
		{
			// Arrange
			var value = "[{\"name\": \"Item 1\"},{\"id\": \"2\", \"name\": \"Item 2\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("2", "Item 2")
			});
		}

		[Fact]
		public void Items_with_missing_name_are_included()
		{
			// Arrange
			var value = "[{\"id\": \"1\"},{\"id\": \"2\", \"name\": \"Item 2\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("1", string.Empty),
				new KeyValuePair<string, string>("2", "Item 2")
			});
		}

		[Fact]
		public void Write()
		{
			// Arrange
			var value = new[]
			{
				new KeyValuePair<string, string>("1", "Audio Quality"),
				new KeyValuePair<string, string>("2", "Video Quality")
			};
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			converter.Write(jsonWriter, value, options);
			jsonWriter.Flush();

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			// Assert
			result.ShouldBe("[{\"id\":\"1\",\"name\":\"Audio Quality\"},{\"id\":\"2\",\"name\":\"Video Quality\"}]");
		}

		[Fact]
		public void Write_NullValue_WritesNothing()
		{
			// Arrange
			KeyValuePair<string, string>[] value = null;
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

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
		public void Write_EmptyArray()
		{
			// Arrange
			var value = Array.Empty<KeyValuePair<string, string>>();
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

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
		public void RoundTrip_MultipleItems()
		{
			// Arrange
			var originalValue = new[]
			{
				new KeyValuePair<string, string>("1", "Audio Quality"),
				new KeyValuePair<string, string>("2", "Video Quality"),
				new KeyValuePair<string, string>("3", "Screen Share Quality")
			};

			// Serialize
			var ms = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(ms);
			var converter = new MeetingParticipantFeedbackDetailsConverter();
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
		public void RealWorld_FeedbackCategories()
		{
			// Arrange - Typical feedback categories from meeting participant feedback
			var json = "[" +
				"{\"id\":\"1\",\"name\":\"Audio Quality\"}," +
				"{\"id\":\"2\",\"name\":\"Video Quality\"}," +
				"{\"id\":\"3\",\"name\":\"Screen Share Quality\"}," +
				"{\"id\":\"4\",\"name\":\"Connection Quality\"}," +
				"{\"id\":\"5\",\"name\":\"Overall Experience\"}" +
				"]";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, null, new JsonSerializerOptions());

			// Assert
			result.ShouldNotBeNull();
			result.Length.ShouldBe(5);
			result[0].Key.ShouldBe("1");
			result[0].Value.ShouldBe("Audio Quality");
			result[1].Key.ShouldBe("2");
			result[1].Value.ShouldBe("Video Quality");
			result[2].Key.ShouldBe("3");
			result[2].Value.ShouldBe("Screen Share Quality");
			result[3].Key.ShouldBe("4");
			result[3].Value.ShouldBe("Connection Quality");
			result[4].Key.ShouldBe("5");
			result[4].Value.ShouldBe("Overall Experience");
		}

		[Fact]
		public void Read_WithSpecialCharacters()
		{
			// Arrange
			var value = "[{\"id\": \"1\", \"name\": \"What's the audio quality?\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("1", "What's the audio quality?")
			});
		}

		[Fact]
		public void Read_WithNumericIds()
		{
			// Arrange
			var value = "[{\"id\": \"123\", \"name\": \"Category 123\"},{\"id\": \"456\", \"name\": \"Category 456\"}]";
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(value);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = new JsonSerializerOptions();

			var converter = new MeetingParticipantFeedbackDetailsConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBe(new[]
			{
				new KeyValuePair<string, string>("123", "Category 123"),
				new KeyValuePair<string, string>("456", "Category 456")
			});
		}
	}
}
