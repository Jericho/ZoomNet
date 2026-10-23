using Shouldly;
using System;
using System.Text;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Json
{
	public class WebinarConverterTests
	{
		[Fact]
		public void Read_RegularWebinar()
		{
			// Arrange
			var json = @"{
				""id"": 123456789,
				""uuid"": ""abc123"",
				""host_id"": ""hostId123"",
				""topic"": ""Test Webinar"",
				""type"": 5,
				""duration"": 60,
				""agenda"": ""Test Agenda"",
				""join_url"": ""https://zoom.us/j/123456789""
			}";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions();

			// Act
			jsonReader.Read();
			var converter = new WebinarConverter();
			var result = converter.Read(ref jsonReader, typeof(Webinar), options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ScheduledWebinar>();
			var scheduledWebinar = (ScheduledWebinar)result;
			scheduledWebinar.Id.ShouldBe(123456789);
			scheduledWebinar.Topic.ShouldBe("Test Webinar");
			scheduledWebinar.Type.ShouldBe(WebinarType.Regular);
			scheduledWebinar.Duration.ShouldBe(60);
		}

		[Fact]
		public void Read_RecurringWebinarWithNoFixedTime()
		{
			// Arrange - Type 6 is RecurringNoFixedTime
			var json = @"{
				""id"": 987654321,
				""uuid"": ""xyz789"",
				""host_id"": ""hostId456"",
				""topic"": ""Recurring Webinar No Fixed Time"",
				""type"": 6,
				""duration"": 60,
				""join_url"": ""https://zoom.us/j/987654321""
			}";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions();

			// Act
			jsonReader.Read();
			var converter = new WebinarConverter();
			var result = converter.Read(ref jsonReader, typeof(Webinar), options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RecurringWebinar>();
			var recurringWebinar = (RecurringWebinar)result;
			recurringWebinar.Id.ShouldBe(987654321);
			recurringWebinar.Topic.ShouldBe("Recurring Webinar No Fixed Time");
			recurringWebinar.Type.ShouldBe(WebinarType.RecurringNoFixedTime);
		}

		[Fact]
		public void Read_RecurringWebinarWithFixedTime()
		{
			// Arrange - Type 9 is RecurringFixedTime
			var json = @"{
				""id"": 111222333,
				""uuid"": ""def456"",
				""host_id"": ""hostId789"",
				""topic"": ""Recurring Webinar Fixed Time"",
				""type"": 9,
				""join_url"": ""https://zoom.us/j/111222333""
			}";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions();

			// Act
			jsonReader.Read();
			var converter = new WebinarConverter();
			var result = converter.Read(ref jsonReader, typeof(Webinar), options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RecurringWebinar>();
			var recurringWebinar = (RecurringWebinar)result;
			recurringWebinar.Id.ShouldBe(111222333);
			recurringWebinar.Topic.ShouldBe("Recurring Webinar Fixed Time");
			recurringWebinar.Type.ShouldBe(WebinarType.RecurringFixedTime);
		}

		[Fact]
		public void Read_UnknownWebinarType_ThrowsException()
		{
			Action lambda = () =>
			{
				// Arrange
				var json = @"{
					""id"": 123456789,
					""uuid"": ""abc123"",
					""host_id"": ""hostId123"",
					""topic"": ""Test Webinar"",
					""type"": 999,
					""duration"": 60
				}";

				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var options = new JsonSerializerOptions();

				// Act
				jsonReader.Read();
				var converter = new WebinarConverter();
				var result = converter.Read(ref jsonReader, typeof(Webinar), options);
			};

			// Assert
			lambda.ShouldThrow<JsonException>()
				.Message.ShouldContain("is an unknown webinar type");
		}

		[Fact]
		public void Read_WebinarWithMinimalFields()
		{
			// Arrange
			var json = @"{
				""id"": 123,
				""type"": 5
			}";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions();

			// Act
			jsonReader.Read();
			var converter = new WebinarConverter();
			var result = converter.Read(ref jsonReader, typeof(Webinar), options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ScheduledWebinar>();
			result.Id.ShouldBe(123);
			result.Type.ShouldBe(WebinarType.Regular);
		}

		[Fact]
		public void Read_RegularWebinarWithDetailedFields()
		{
			// Arrange
			var json = @"{
				""id"": 123456789,
				""uuid"": ""abc123uuid"",
				""host_id"": ""hostId123"",
				""host_email"": ""host@example.com"",
				""topic"": ""Comprehensive Test Webinar"",
				""type"": 5,
				""duration"": 90,
				""agenda"": ""Detailed test agenda"",
				""join_url"": ""https://zoom.us/j/123456789"",
				""password"": ""secret123""
			}";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions();

			// Act
			jsonReader.Read();
			var converter = new WebinarConverter();
			var result = converter.Read(ref jsonReader, typeof(Webinar), options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ScheduledWebinar>();
			var scheduledWebinar = (ScheduledWebinar)result;
			scheduledWebinar.Id.ShouldBe(123456789);
			scheduledWebinar.Uuid.ShouldBe("abc123uuid");
			scheduledWebinar.HostId.ShouldBe("hostId123");
			scheduledWebinar.Topic.ShouldBe("Comprehensive Test Webinar");
			scheduledWebinar.Type.ShouldBe(WebinarType.Regular);
			scheduledWebinar.Duration.ShouldBe(90);
			scheduledWebinar.Agenda.ShouldBe("Detailed test agenda");
			scheduledWebinar.Password.ShouldBe("secret123");
		}

		[Fact]
		public void Read_MissingTypeField_ThrowsException()
		{
			Action lambda = () =>
			{
				// Arrange
				var json = @"{
					""id"": 123456789,
					""uuid"": ""abc123"",
					""topic"": ""Test Webinar""
				}";

				var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
				var jsonReader = new Utf8JsonReader(jsonUtf8);
				var options = new JsonSerializerOptions();

				// Act
				jsonReader.Read();
				var converter = new WebinarConverter();
				var result = converter.Read(ref jsonReader, typeof(Webinar), options);
			};

			// Assert
			lambda.ShouldThrow<Exception>(); // Will throw when trying to get the type property
		}

		[Fact]
		public void Read_RegularWebinarWithUnicodeCharacters()
		{
			// Arrange
			var json = @"{
				""id"": 123456789,
				""type"": 5,
				""topic"": ""ウェビナー - Webinar en Français"",
				""agenda"": ""日本語のアジェンダ""
			}";

			var jsonUtf8 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json);
			var jsonReader = new Utf8JsonReader(jsonUtf8);
			var options = new JsonSerializerOptions();

			// Act
			jsonReader.Read();
			var converter = new WebinarConverter();
			var result = converter.Read(ref jsonReader, typeof(Webinar), options);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ScheduledWebinar>();
			result.Topic.ShouldBe("ウェビナー - Webinar en Français");
			((ScheduledWebinar)result).Agenda.ShouldBe("日本語のアジェンダ");
		}

		[Fact]
		public void Read_ConvertsToCorrectType_BasedOnWebinarType()
		{
			// Test that type 5 returns ScheduledWebinar
			var json5 = @"{""id"": 1, ""type"": 5}";
			var jsonUtf85 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json5);
			var jsonReader5 = new Utf8JsonReader(jsonUtf85);
			jsonReader5.Read();
			var converter5 = new WebinarConverter();
			var result5 = converter5.Read(ref jsonReader5, typeof(Webinar), new JsonSerializerOptions());
			result5.ShouldBeOfType<ScheduledWebinar>();

			// Test that type 6 returns RecurringWebinar (RecurringNoFixedTime)
			var json6 = @"{""id"": 2, ""type"": 6}";
			var jsonUtf86 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json6);
			var jsonReader6 = new Utf8JsonReader(jsonUtf86);
			jsonReader6.Read();
			var converter6 = new WebinarConverter();
			var result6 = converter6.Read(ref jsonReader6, typeof(Webinar), new JsonSerializerOptions());
			result6.ShouldBeOfType<RecurringWebinar>();
			result6.Type.ShouldBe(WebinarType.RecurringNoFixedTime);

			// Test that type 9 returns RecurringWebinar (RecurringFixedTime)
			var json9 = @"{""id"": 3, ""type"": 9}";
			var jsonUtf89 = (ReadOnlySpan<byte>)Encoding.UTF8.GetBytes(json9);
			var jsonReader9 = new Utf8JsonReader(jsonUtf89);
			jsonReader9.Read();
			var converter9 = new WebinarConverter();
			var result9 = converter9.Read(ref jsonReader9, typeof(Webinar), new JsonSerializerOptions());
			result9.ShouldBeOfType<RecurringWebinar>();
			result9.Type.ShouldBe(WebinarType.RecurringFixedTime);
		}
	}
}
