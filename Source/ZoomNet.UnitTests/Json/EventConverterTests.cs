using Shouldly;
using System;
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
		public void Throws_whe_event_type_is_invalid()
		{
			// Arrange
			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(INVALID_EVENT_TYPE_JSON);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var objectType = (Type)null;
			var options = JsonFormatter.DefaultDeserializerOptions;

			var converter = new EventConverter();

			// Act
			jsonReader.Read();

			try
			{
				var result = converter.Read(ref jsonReader, objectType, options);
			}
			catch (JsonException e)
			{
				e.Message.ShouldBe("HELLO_WORLD is an unknown event type");

				// Unfortunately, cannot use Should.Throw<JsonException>(() => converter.Read(ref jsonReader, objectType, options));
				// because we can't use 'ref' arguments in lambda expressions.
			}
		}
	}
}
