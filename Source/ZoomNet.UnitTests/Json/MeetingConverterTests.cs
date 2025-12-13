using Shouldly;
using System;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Json
{
	public class MeetingConverterTests
	{
		private const string INSTANT_MEETING_JSON = @"{
			""uuid"": ""abc123"",
			""id"": 123456789,
			""host_id"": ""hostId123"",
			""topic"": ""Instant Meeting"",
			""type"": 1,
			""status"": ""waiting"",
			""start_url"": ""https://zoom.us/s/123456789?zak=token"",
			""join_url"": ""https://zoom.us/j/123456789"",
			""created_at"": ""2023-12-15T10:30:00Z""
		}";

		private const string PERSONAL_MEETING_JSON = @"{
			""uuid"": ""def456"",
			""id"": 987654321,
			""host_id"": ""hostId456"",
			""topic"": ""Personal Meeting Room"",
			""type"": 4,
			""status"": ""waiting"",
			""start_url"": ""https://zoom.us/s/987654321?zak=token"",
			""join_url"": ""https://zoom.us/j/987654321"",
			""created_at"": ""2023-12-15T11:00:00Z""
		}";

		private const string SCHEDULED_MEETING_JSON = @"{
			""uuid"": ""ghi789"",
			""id"": 111222333,
			""host_id"": ""hostId789"",
			""topic"": ""Scheduled Meeting"",
			""type"": 2,
			""status"": ""waiting"",
			""start_time"": ""2023-12-20T14:00:00Z"",
			""duration"": 60,
			""timezone"": ""America/Los_Angeles"",
			""start_url"": ""https://zoom.us/s/111222333?zak=token"",
			""join_url"": ""https://zoom.us/j/111222333"",
			""created_at"": ""2023-12-15T12:00:00Z"",
			""pmi"": ""1234567890"",
			""pre_scheduled"": false
		}";

		private const string RECURRING_MEETING_NO_FIXED_TIME_JSON = @"{
			""uuid"": ""jkl012"",
			""id"": 444555666,
			""host_id"": ""hostId012"",
			""topic"": ""Recurring Meeting No Fixed Time"",
			""type"": 3,
			""status"": ""waiting"",
			""start_url"": ""https://zoom.us/s/444555666?zak=token"",
			""join_url"": ""https://zoom.us/j/444555666"",
			""created_at"": ""2023-12-15T13:00:00Z"",
			""pmi"": ""9876543210"",
			""recurrence"": {
				""type"": 1,
				""repeat_interval"": 1
			},
			""pre_scheduled"": false
		}";

		private const string RECURRING_MEETING_FIXED_TIME_JSON = @"{
			""uuid"": ""mno345"",
			""id"": 777888999,
			""host_id"": ""hostId345"",
			""topic"": ""Recurring Meeting Fixed Time"",
			""type"": 8,
			""status"": ""waiting"",
			""start_url"": ""https://zoom.us/s/777888999?zak=token"",
			""join_url"": ""https://zoom.us/j/777888999"",
			""created_at"": ""2023-12-15T14:00:00Z"",
			""pmi"": ""5555555555"",
			""recurrence"": {
				""type"": 2,
				""repeat_interval"": 1,
				""weekly_days"": ""1,3,5""
			},
			""occurrences"": [
				{
					""occurrence_id"": ""1482205800000"",
					""start_time"": ""2023-12-20T10:00:00Z"",
					""duration"": 30,
					""status"": ""available""
				}
			],
			""pre_scheduled"": true
		}";

		[Fact]
		public void Read_InstantMeeting()
		{
			// Arrange
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(INSTANT_MEETING_JSON, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<InstantMeeting>();
			result.Type.ShouldBe(MeetingType.Instant);
			result.Id.ShouldBe(123456789);
			result.Topic.ShouldBe("Instant Meeting");
			result.HostId.ShouldBe("hostId123");
			result.Status.ShouldBe(MeetingStatus.Waiting);
		}

		[Fact]
		public void Read_PersonalMeeting()
		{
			// Arrange
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(PERSONAL_MEETING_JSON, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<InstantMeeting>();
			result.Type.ShouldBe(MeetingType.Personal);
			result.Id.ShouldBe(987654321);
			result.Topic.ShouldBe("Personal Meeting Room");
			result.HostId.ShouldBe("hostId456");
		}

		[Fact]
		public void Read_ScheduledMeeting()
		{
			// Arrange
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(SCHEDULED_MEETING_JSON, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ScheduledMeeting>();
			result.Type.ShouldBe(MeetingType.Scheduled);
			result.Id.ShouldBe(111222333);
			result.Topic.ShouldBe("Scheduled Meeting");

			var scheduledMeeting = result as ScheduledMeeting;
			scheduledMeeting.ShouldNotBeNull();
			scheduledMeeting.StartTime.ShouldBe(new DateTime(2023, 12, 20, 14, 0, 0, DateTimeKind.Utc));
			scheduledMeeting.Duration.ShouldBe(60);
			scheduledMeeting.PersonalMeetingId.ShouldBe("1234567890");
			scheduledMeeting.PreScheduled.ShouldBeFalse();
		}

		[Fact]
		public void Read_RecurringMeetingNoFixedTime()
		{
			// Arrange
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(RECURRING_MEETING_NO_FIXED_TIME_JSON, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RecurringMeeting>();
			result.Type.ShouldBe(MeetingType.RecurringNoFixedTime);
			result.Id.ShouldBe(444555666);
			result.Topic.ShouldBe("Recurring Meeting No Fixed Time");

			var recurringMeeting = result as RecurringMeeting;
			recurringMeeting.ShouldNotBeNull();
			recurringMeeting.PersonalMeetingId.ShouldBe("9876543210");
			recurringMeeting.RecurrenceInfo.ShouldNotBeNull();
			recurringMeeting.RecurrenceInfo.Type.ShouldBe(RecurrenceType.Daily);
			recurringMeeting.PreScheduled.ShouldBeFalse();
		}

		[Fact]
		public void Read_RecurringMeetingFixedTime()
		{
			// Arrange
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(RECURRING_MEETING_FIXED_TIME_JSON, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RecurringMeeting>();
			result.Type.ShouldBe(MeetingType.RecurringFixedTime);
			result.Id.ShouldBe(777888999);
			result.Topic.ShouldBe("Recurring Meeting Fixed Time");

			var recurringMeeting = result as RecurringMeeting;
			recurringMeeting.ShouldNotBeNull();
			recurringMeeting.PersonalMeetingId.ShouldBe("5555555555");
			recurringMeeting.RecurrenceInfo.ShouldNotBeNull();
			recurringMeeting.RecurrenceInfo.Type.ShouldBe(RecurrenceType.Weekly);
			recurringMeeting.Occurrences.ShouldNotBeNull();
			recurringMeeting.Occurrences.ShouldHaveSingleItem();
			recurringMeeting.PreScheduled.ShouldBeTrue();
		}

		[Fact]
		public void Read_ThrowsWhenUnknownMeetingType()
		{
			// Arrange
			var invalidJson = @"{
				""uuid"": ""xyz999"",
				""id"": 999999999,
				""host_id"": ""hostId999"",
				""topic"": ""Invalid Meeting"",
				""type"": 99,
				""created_at"": ""2023-12-15T15:00:00Z""
			}";
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			void DeserializeMeeting()
			{
				JsonSerializer.Deserialize<Meeting>(invalidJson, options);
			}

			// Assert
			var exception = Should.Throw<JsonException>(DeserializeMeeting);
			exception.Message.ShouldContain("unknown meeting type");
		}

		[Fact]
		public void Read_ThrowsWhenMeetingTypeMissing()
		{
			// Arrange
			var invalidJson = @"{
				""uuid"": ""xyz888"",
				""id"": 888888888,
				""host_id"": ""hostId888"",
				""topic"": ""Meeting Without Type"",
				""created_at"": ""2023-12-15T15:30:00Z""
			}";
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			void DeserializeMeeting()
			{
				JsonSerializer.Deserialize<Meeting>(invalidJson, options);
			}

			// Assert
			Should.Throw<Exception>(DeserializeMeeting);
		}

		[Fact]
		public void Read_InstantMeeting_WithAllProperties()
		{
			// Arrange
			var detailedJson = @"{
				""uuid"": ""abc123"",
				""id"": 123456789,
				""host_id"": ""hostId123"",
				""topic"": ""Detailed Instant Meeting"",
				""type"": 1,
				""status"": ""started"",
				""start_url"": ""https://zoom.us/s/123456789?zak=token"",
				""join_url"": ""https://zoom.us/j/123456789"",
				""password"": ""pass123"",
				""h323_password"": ""h323pass"",
				""pstn_password"": ""pstnpass"",
				""encrypted_password"": ""encryptedpass123"",
				""created_at"": ""2023-12-15T10:30:00Z"",
				""timezone"": ""America/New_York"",
				""agenda"": ""Discuss project updates"",
				""host_email"": ""host@example.com"",
				""assistant_id"": ""assistant123"",
				""settings"": {
					""join_before_host"": true
				}
			}";
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(detailedJson, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<InstantMeeting>();
			result.Type.ShouldBe(MeetingType.Instant);
			result.Password.ShouldBe("pass123");
			result.H323Password.ShouldBe("h323pass");
			result.PstnPassword.ShouldBe("pstnpass");
			result.EncryptedPassword.ShouldBe("encryptedpass123");
			result.Timezone.ShouldBe(TimeZones.America_New_York);
			result.Agenda.ShouldBe("Discuss project updates");
			result.HostEmail.ShouldBe("host@example.com");
			result.AssistantId.ShouldBe("assistant123");
			result.Settings.ShouldNotBeNull();
			result.Settings.JoinBeforeHost.ShouldNotBeNull();
			result.Settings.JoinBeforeHost.Value.ShouldBeTrue();
		}

		[Theory]
		[InlineData(1, typeof(InstantMeeting))]
		[InlineData(2, typeof(ScheduledMeeting))]
		[InlineData(3, typeof(RecurringMeeting))]
		[InlineData(4, typeof(InstantMeeting))]
		[InlineData(8, typeof(RecurringMeeting))]
		public void Read_CorrectTypeForMeetingType(int meetingTypeValue, Type expectedType)
		{
			// Arrange
			var json = $@"{{
				""type"": {meetingTypeValue}
			}}";
			var options = JsonFormatter.DefaultDeserializerOptions;

			// Act
			var result = JsonSerializer.Deserialize<Meeting>(json, options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType(expectedType);
		}
	}
}
