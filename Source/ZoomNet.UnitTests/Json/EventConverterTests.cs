using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Json
{
	public class EventConverterTests
	{
		// This sample JSON was copied from Zoom's documentation: https://developers.zoom.us/docs/api/events/#tag/events/get/zoom_events/events/{eventId}
		private const string SAMPLE_EVENT_JSON = @"
  ""event_id"": ""234kj2h34kljgh23lkhj3"",
  ""name"": ""OpenAPI Conference Name"",
  ""description"": ""This event was created with the OpenAPI."",
  ""timezone"": ""America/Indianapolis"",
  ""recurrence"": {
    ""type"": 1,
    ""repeat_interval"": 1,
    ""weekly_days"": ""1,2,3"",
    ""monthly_days"": 1,
    ""monthly_week_day"": 1,
    ""end_times"": 1,
    ""end_date_time"": ""2025-09-17T16:23:40.184Z"",
    ""monthly_week"": -1,
    ""duration"": 1
  },
  ""access_level"": ""PRIVATE_RESTRICTED"",
  ""meeting_type"": ""MEETING"",
  ""categories"": [
    ""Food and Drinks""
  ],
  ""tags"": [
    ""Event tag1""
  ],
  ""calendar"": [
    {
      ""start_time"": ""2022-06-03T20:51:00Z"",
      ""end_time"": ""2022-06-03T20:51:00Z""
    }
  ],
  ""status"": ""PUBLISHED"",
  ""hub_id"": ""23asdfasdf3asdf"",
  ""host_id"": ""XMgGb1i6Qlah8mn3e5GYMX"",
  ""start_time"": ""2022-06-03T20:51:00Z"",
  ""end_time"": ""2022-06-03T20:51:00Z"",
  ""contact_name"": ""user contact name"",
  ""contact_email"": ""user@zoom.us"",
  ""lobby_start_time"": ""2022-06-03T20:51:00Z"",
  ""lobby_end_time"": ""2022-06-03T20:51:00Z"",
  ""event_url"": ""www.example.com/zoomEvents"",
  ""blocked_countries"": [
    ""US""
  ],
  ""attendance_type"": ""hybrid"",
  ""tagline"": ""Unlocking Innovation: Join Us for the Day of Insipiration and Insight!""";

		private const string SIMPLE_EVENT_JSON = "{\"event_type\": \"SIMPLE_EVENT\"," + SAMPLE_EVENT_JSON + "}";
		private const string CONFERENCE_EVENT_JSON = "{\"event_type\": \"CONFERENCE\"," + SAMPLE_EVENT_JSON + "}";
		private const string RECURRING_EVENT_JSON = "{\"event_type\": \"RECURRING\"," + SAMPLE_EVENT_JSON + "}";
		private const string INVALID_EVENT_TYPE_JSON = "{\"event_type\": \"HELLO_WORLD\"," + SAMPLE_EVENT_JSON + "}";
		private const string MISSING_EVENT_TYPE_JSON = "{" + SAMPLE_EVENT_JSON + "}";

		[Theory]
		[InlineData(SIMPLE_EVENT_JSON, typeof(SimpleEvent))]
		[InlineData(CONFERENCE_EVENT_JSON, typeof(Conference))]
		[InlineData(RECURRING_EVENT_JSON, typeof(RecurringEvent))]
		public void Read(string json, Type expectedType)
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new EventConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options);

			// Assert
			result.ShouldBeOfType(expectedType);
		}

		[Fact]
		public void Read_SimpleEvent_DeserializesCorrectly()
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(SIMPLE_EVENT_JSON);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new EventConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options) as SimpleEvent;

			// Assert
			result.ShouldNotBeNull();
			result.Type.ShouldBe(EventType.Simple);
			result.Id.ShouldBe("234kj2h34kljgh23lkhj3");
			result.Name.ShouldBe("OpenAPI Conference Name");
		}

		[Fact]
		public void Read_Conference_DeserializesCorrectly()
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(CONFERENCE_EVENT_JSON);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new EventConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options) as Conference;

			// Assert
			result.ShouldNotBeNull();
			result.Type.ShouldBe(EventType.Conference);
			result.Id.ShouldBe("234kj2h34kljgh23lkhj3");
			result.Name.ShouldBe("OpenAPI Conference Name");
		}

		[Fact]
		public void Read_RecurringEvent_DeserializesCorrectly()
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(RECURRING_EVENT_JSON);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new EventConverter();

			// Act
			jsonReader.Read();
			var result = converter.Read(ref jsonReader, objectType, options) as RecurringEvent;

			// Assert
			result.ShouldNotBeNull();
			result.Type.ShouldBe(EventType.Reccuring);
			result.Id.ShouldBe("234kj2h34kljgh23lkhj3");
			result.Name.ShouldBe("OpenAPI Conference Name");
		}

		[Fact]
		public void Read_Throws_When_EventType_Is_Invalid()
		{
			Action lambda = () =>
			{
				// Arrange
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(INVALID_EVENT_TYPE_JSON);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = JsonFormatter.DefaultDeserializerOptions;

				var converter = new EventConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrowWithMessage<JsonException>("HELLO_WORLD is an unknown event type");
		}

		[Fact]
		public void Read_Throws_When_EventType_Is_Missing()
		{
			// Arrange
			Action lambda = () =>
			{
				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(MISSING_EVENT_TYPE_JSON);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var objectType = (Type)null;
				var options = JsonFormatter.DefaultDeserializerOptions;

				var converter = new EventConverter();

				// Act
				jsonReader.Read();
				var result = converter.Read(ref jsonReader, objectType, options);
			};

			// Assert
			lambda.ShouldThrow<Exception>("Unable to find event_type in the Json document");
		}

		[Fact]
		public void Write_SimpleEvent_SerializesCorrectly()
		{
			// Arrange
			var simpleEvent = new SimpleEvent
			{
				Id = "test123",
				Name = "Test Event",
				Type = EventType.Simple,
				MeetingType = EventMeetingType.Meeting
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act
			converter.Write(writer, simpleEvent, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"event_id\":\"test123\"");
			json.ShouldContain("\"name\":\"Test Event\"");
			json.ShouldContain("\"event_type\":\"SIMPLE_EVENT\"");
		}

		[Fact]
		public void Write_Conference_SerializesCorrectly()
		{
			// Arrange
			var conference = new Conference
			{
				Id = "conf456",
				Name = "Test Conference",
				Type = EventType.Conference
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act
			converter.Write(writer, conference, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"event_id\":\"conf456\"");
			json.ShouldContain("\"name\":\"Test Conference\"");
			json.ShouldContain("\"event_type\":\"CONFERENCE\"");
		}

		[Fact]
		public void Write_RecurringEvent_SerializesCorrectly()
		{
			// Arrange
			var recurringEvent = new RecurringEvent
			{
				Id = "rec789",
				Name = "Test Recurring Event",
				Type = EventType.Reccuring,
				RecurrenceInfo = new EventRecurrenceInfo { Type = RecurrenceType.Daily }
			};

			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act
			converter.Write(writer, recurringEvent, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldContain("\"event_id\":\"rec789\"");
			json.ShouldContain("\"name\":\"Test Recurring Event\"");
			json.ShouldContain("\"event_type\":\"RECURRING\"");
		}

		[Fact]
		public void Write_NullEvent_SerializesAsNull()
		{
			// Arrange
			Event nullEvent = null;
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act
			converter.Write(writer, nullEvent, options);
			writer.Flush();
			var json = Encoding.UTF8.GetString(stream.ToArray());

			// Assert
			json.ShouldBe("null");
		}

		[Fact]
		public void RoundTrip_SimpleEvent_PreservesData()
		{
			// Arrange
			var originalEvent = new SimpleEvent
			{
				Id = "roundtrip123",
				Name = "RoundTrip Test",
				Description = "Test Description",
				Type = EventType.Simple,
				MeetingType = EventMeetingType.Webinar,
				HubId = "hub123",
				ContactName = "John Doe"
			};

			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act - Serialize
			using var writeStream = new MemoryStream();
			using (var writer = new Utf8JsonWriter(writeStream))
			{
				converter.Write(writer, originalEvent, options);
			}

			var json = Encoding.UTF8.GetString(writeStream.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserializedEvent = converter.Read(ref jsonReader, typeof(Event), options) as SimpleEvent;

			// Assert
			deserializedEvent.ShouldNotBeNull();
			deserializedEvent.Id.ShouldBe(originalEvent.Id);
			deserializedEvent.Name.ShouldBe(originalEvent.Name);
			deserializedEvent.Description.ShouldBe(originalEvent.Description);
			deserializedEvent.Type.ShouldBe(originalEvent.Type);
			deserializedEvent.MeetingType.ShouldBe(originalEvent.MeetingType);
			deserializedEvent.HubId.ShouldBe(originalEvent.HubId);
			deserializedEvent.ContactName.ShouldBe(originalEvent.ContactName);
		}

		[Fact]
		public void RoundTrip_Conference_PreservesData()
		{
			// Arrange
			var originalEvent = new Conference
			{
				Id = "conference789",
				Name = "Conference RoundTrip",
				Description = "Conference Description",
				Type = EventType.Conference,
				HubId = "hub456",
				ContactName = "Jane Smith",
				TagLine = "Conference TagLine"
			};

			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act - Serialize
			using var writeStream = new MemoryStream();
			using (var writer = new Utf8JsonWriter(writeStream))
			{
				converter.Write(writer, originalEvent, options);
			}

			var json = Encoding.UTF8.GetString(writeStream.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserializedEvent = converter.Read(ref jsonReader, typeof(Event), options) as Conference;

			// Assert
			deserializedEvent.ShouldNotBeNull();
			deserializedEvent.Id.ShouldBe(originalEvent.Id);
			deserializedEvent.Name.ShouldBe(originalEvent.Name);
			deserializedEvent.Description.ShouldBe(originalEvent.Description);
			deserializedEvent.Type.ShouldBe(originalEvent.Type);
			deserializedEvent.HubId.ShouldBe(originalEvent.HubId);
			deserializedEvent.ContactName.ShouldBe(originalEvent.ContactName);
			deserializedEvent.TagLine.ShouldBe(originalEvent.TagLine);
		}

		[Fact]
		public void RoundTrip_RecurringEvent_PreservesData()
		{
			// Arrange
			var originalEvent = new RecurringEvent
			{
				Id = "recurring999",
				Name = "Recurring RoundTrip",
				Description = "Recurring Description",
				Type = EventType.Reccuring,
				HubId = "hub789",
				ContactName = "Bob Johnson",
				RecurrenceInfo = new EventRecurrenceInfo
				{
					Type = RecurrenceType.Weekly,
					RepeatInterval = 1
				}
			};

			var options = JsonFormatter.DefaultSerializerOptions;
			var converter = new EventConverter();

			// Act - Serialize
			using var writeStream = new MemoryStream();
			using (var writer = new Utf8JsonWriter(writeStream))
			{
				converter.Write(writer, originalEvent, options);
			}

			var json = Encoding.UTF8.GetString(writeStream.ToArray());

			// Act - Deserialize
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			jsonReader.Read();
			var deserializedEvent = converter.Read(ref jsonReader, typeof(Event), options) as RecurringEvent;

			// Assert
			deserializedEvent.ShouldNotBeNull();
			deserializedEvent.Id.ShouldBe(originalEvent.Id);
			deserializedEvent.Name.ShouldBe(originalEvent.Name);
			deserializedEvent.Description.ShouldBe(originalEvent.Description);
			deserializedEvent.Type.ShouldBe(originalEvent.Type);
			deserializedEvent.HubId.ShouldBe(originalEvent.HubId);
			deserializedEvent.ContactName.ShouldBe(originalEvent.ContactName);
			deserializedEvent.RecurrenceInfo.ShouldNotBeNull();
			deserializedEvent.RecurrenceInfo.Type.ShouldBe(originalEvent.RecurrenceInfo.Type);
		}
	}
}
