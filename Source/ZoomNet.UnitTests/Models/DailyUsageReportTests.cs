using Shouldly;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class DailyUsageReportTests
	{
		#region FIELDS

		internal const string DAILY_USAGE_REPORT_JSON = @"{
			""dates"": [
				{
					""date"": ""2022-03-01"",
					""meeting_minutes"": 34,
					""meetings"": 2,
					""new_users"": 3,
					""participants"": 4
				}
			],
			""month"": 3,
			""year"": 2022
		}";

		#endregion

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<DailyUsageReport>(DAILY_USAGE_REPORT_JSON, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Month.ShouldBe(3);
			result.Year.ShouldBe(2022);
			result.DailyUsageSummaries.ShouldNotBeEmpty();
			result.DailyUsageSummaries.Length.ShouldBe(1);
			result.DailyUsageSummaries[0].Date.Year.ShouldBe(2022);
			result.DailyUsageSummaries[0].Date.Month.ShouldBe(3);
			result.DailyUsageSummaries[0].Date.Day.ShouldBe(1);
			result.DailyUsageSummaries[0].MeetingMinutes.ShouldBe(34);
			result.DailyUsageSummaries[0].Meetings.ShouldBe(2);
			result.DailyUsageSummaries[0].NewUsers.ShouldBe(3);
			result.DailyUsageSummaries[0].Participants.ShouldBe(4);

		}
	}
}
