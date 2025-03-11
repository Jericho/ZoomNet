using Shouldly;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Utilities;
using static ZoomNet.Internal;

namespace ZoomNet.UnitTests.Extensions
{
	public class InternalTests
	{
		public class FromUnixTime
		{
			// Note to self:
			// I'm using TheoryData because can't use DateTime with InlineData: 
			// Error CS0182  An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
			public static TheoryData<long, DateTime> FromMilliseconds = new TheoryData<long, DateTime>()
			{
				{ 0, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
				{ 1000, new DateTime(1970, 1, 1, 0, 0, 1, 0, DateTimeKind.Utc) },
				{ 16040, new DateTime(1970, 1, 1, 0, 0, 16, 40, DateTimeKind.Utc) },
			};

			public static TheoryData<long, DateTime> FromSeconds = new TheoryData<long, DateTime>()
			{
				{ 0, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
				{ 1000, new DateTime(1970, 1, 1, 0, 16, 40, 0, DateTimeKind.Utc) },
			};

			[Theory, MemberData(nameof(FromMilliseconds))]
			public void Converts_from_milliseconds(long numberOfMilliseconds, DateTime expected)
			{
				// Act
				var result = numberOfMilliseconds.FromUnixTime(UnixTimePrecision.Milliseconds);

				// Assert
				result.ShouldBe(expected);
			}

			[Theory, MemberData(nameof(FromSeconds))]
			public void Converts_from_seconds(long numberOfSeconds, DateTime expected)
			{
				// Act
				var result = numberOfSeconds.FromUnixTime(UnixTimePrecision.Seconds);

				// Assert
				result.ShouldBe(expected);
			}

			[Fact]
			public void Throws_when_unknown_precision()
			{
				// Arrange
				var unknownPrecision = (UnixTimePrecision)3;

				// Act
				Should.Throw<ArgumentException>(() => 123L.FromUnixTime(unknownPrecision));
			}
		}

		public class ToUnixTime
		{
			// Note to self:
			// I'm using TheoryData because can't use DateTime with InlineData: 
			// Error CS0182  An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
			public static TheoryData<DateTime, long> ToMilliseconds = new TheoryData<DateTime, long>()
			{
				{ new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
				{ new DateTime(1970, 1, 1, 0, 0, 1, 0, DateTimeKind.Utc), 1000 },
				{ new DateTime(1970, 1, 1, 0, 0, 16, 40, DateTimeKind.Utc), 16040 },
			};

			public static TheoryData<DateTime, long> ToSeconds = new TheoryData<DateTime, long>()
			{
				{ new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
				{ new DateTime(1970, 1, 1, 0, 0, 1, 0, DateTimeKind.Utc), 1 },
				{ new DateTime(1970, 1, 1, 0, 0, 16, 40, DateTimeKind.Utc), 16 },
			};

			[Theory, MemberData(nameof(ToMilliseconds))]
			public void Converts_to_milliseconds(DateTime date, long expected)
			{
				// Act
				var result = date.ToUnixTime(UnixTimePrecision.Milliseconds);

				// Assert
				result.ShouldBe(expected);
			}

			[Theory, MemberData(nameof(ToSeconds))]
			public void Converts_to_seconds(DateTime date, long expected)
			{
				// Act
				var result = date.ToUnixTime(UnixTimePrecision.Seconds);

				// Assert
				result.ShouldBe(expected);
			}

			[Fact]
			public void Throws_when_unknown_precision()
			{
				// Arrange
				var unknownPrecision = (UnixTimePrecision)3;

				// Act
				Should.Throw<ArgumentException>(() => DateTime.UtcNow.ToUnixTime(unknownPrecision));
			}
		}

		public class ToZoomFormat
		{
			// Note to self:
			// I'm using TheoryData because can't use DateTime with InlineData: 
			// Error CS0182  An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
			public static TheoryData<DateTime, string, string> SampleUtcDates = new TheoryData<DateTime, string, string>()
			{
				{ new DateTime(2023, 12, 12, 12, 14, 0, 0, DateTimeKind.Utc), "2023-12-12", "2023-12-12T12:14:00Z" },
			};

			[Theory, MemberData(nameof(SampleUtcDates))]
			public void Successfully_converts_UTC_to_string(DateTime date, string expectedDateOnly, string expectedWithTime)
			{
				// Act
				var resultDateOnly = date.ToZoomFormat(TimeZones.UTC, true);
				var resultWithTime = date.ToZoomFormat(TimeZones.UTC, false);

				// Assert
				resultDateOnly.ShouldBe(expectedDateOnly);
				resultWithTime.ShouldBe(expectedWithTime);
			}

			// Note to self:
			// I'm using TheoryData because can't use DateTime with InlineData: 
			// Error CS0182  An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
			public static TheoryData<DateTime, string, string> SampleLocalDates = new TheoryData<DateTime, string, string>()
			{
				{ new DateTime(2023, 12, 12, 12, 14, 0, 0, DateTimeKind.Local), "2023-12-12", "2023-12-12T12:14:00" },
			};

			[Theory, MemberData(nameof(SampleLocalDates))]
			public void Successfully_converts_Local_to_string(DateTime date, string expectedDateOnly, string expectedWithTime)
			{
				// Act
				var resultDateOnly = date.ToZoomFormat(TimeZones.America_Montreal, true);
				var resultWithTime = date.ToZoomFormat(TimeZones.America_Montreal, false);

				// Assert
				resultDateOnly.ShouldBe(expectedDateOnly);
				resultWithTime.ShouldBe(expectedWithTime);
			}

			[Fact]
			public void Returns_null_when_null()
			{
				// Arrange
				var date = (DateTime?)null;

				// Act
				var result = date.ToZoomFormat();

				// Assert
				result.ShouldBeNull();
			}
		}

		public class AsPaginatedResponse
		{
			[Fact]
			public async Task ThrowsExceptionWhenExpectedRecordsAreMissing()
			{
				// Arrange
				var responseContent = @"{
					""next_page_token"": """",
					""page_number"": 1,
					""page_size"": 100,
					""total_records"": 5
				}";
				var message = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(responseContent)
				};
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				await Should.ThrowAsync<ArgumentException>(() => response.AsPaginatedResponse<UserCallLog>("call_logs")).ConfigureAwait(true);
			}

			[Fact]
			public async Task DoesNotThrowExceptionWhenRecordsAreMissingAndTotalRecordsIsZero()
			{
				// Arrange
				var responseContent = @"{
					""next_page_token"": """",
					""page_size"": 100,
					""total_records"": 0
				}";
				var message = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(responseContent)
				};
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				var paginatedResponse = await response.AsPaginatedResponse<UserCallLog>("call_logs").ConfigureAwait(true);

				// Assert
				paginatedResponse.PageCount.ShouldBe(0);
				paginatedResponse.PageNumber.ShouldBe(0);
				paginatedResponse.PageSize.ShouldBe(100);
				paginatedResponse.Records.ShouldBeEmpty();
				paginatedResponse.TotalRecords.ShouldBe(0);
			}
		}

		public class AsPaginatedResponseWithToken
		{
			[Fact]
			public async Task ThrowsExceptionWhenExpectedRecordsAreMissing()
			{
				// Arrange
				var responseContent = @"{
					""next_page_token"": """",
					""page_number"": 1,
					""page_size"": 100,
					""total_records"": 5,
					""from"": ""2023-07-05"",
					""to"": ""2023-07-12""
				}";
				var message = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(responseContent)
				};
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				await Should.ThrowAsync<ArgumentException>(() => response.AsPaginatedResponseWithToken<UserCallLog>("call_logs")).ConfigureAwait(true);
			}

			[Fact]
			public async Task DoesNotThrowExceptionWhenRecordsAreMissingAndTotalRecordsIsZero()
			{
				// Arrange
				var responseContent = @"{
					""next_page_token"": """",
					""page_size"": 100,
					""total_records"": 0,
					""from"": ""2023-07-05"",
					""to"": ""2023-07-12""
				}";
				var message = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(responseContent)
				};
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				var paginatedResponse = await response.AsPaginatedResponseWithToken<UserCallLog>("call_logs").ConfigureAwait(true);

				// Assert
				paginatedResponse.PageSize.ShouldBe(100);
				paginatedResponse.Records.ShouldBeEmpty();
				paginatedResponse.TotalRecords.ShouldBe(0);
			}
		}

		public class AsPaginatedResponseWithTokenAndDateRange
		{
			[Fact]
			public async Task ThrowsExceptionWhenExpectedRecordsAreMissing()
			{
				// Arrange
				var responseContent = @"{
					""next_page_token"": """",
					""page_number"": 1,
					""page_size"": 100,
					""total_records"": 5,
					""from"": ""2023-07-05"",
					""to"": ""2023-07-12""
				}";
				var message = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(responseContent)
				};
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				await Should.ThrowAsync<ArgumentException>(() => response.AsPaginatedResponseWithTokenAndDateRange<UserCallLog>("call_logs")).ConfigureAwait(true);
			}

			[Fact]
			public async Task DoesNotThrowExceptionWhenRecordsAreMissingAndTotalRecordsIsZero()
			{
				// Arrange
				var responseContent = @"{
					""next_page_token"": """",
					""page_size"": 100,
					""total_records"": 0,
					""from"": ""2023-07-05"",
					""to"": ""2023-07-12""
				}";
				var message = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(responseContent)
				};
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				var paginatedResponse = await response.AsPaginatedResponseWithTokenAndDateRange<UserCallLog>("call_logs").ConfigureAwait(true);

				// Assert
				paginatedResponse.PageSize.ShouldBe(100);
				paginatedResponse.Records.ShouldBeEmpty();
				paginatedResponse.TotalRecords.ShouldBe(0);
			}
		}

		public class Enumconversion
		{
			public enum MyEnum
			{
				[EnumMember(Value = "One")]
				First = 1,

				[MultipleValuesEnumMember(DefaultValue = "Two", OtherValues = new[] { "Second", "Alternative" })]
				Second = 2,

				[JsonPropertyName("Three")]
				Third = 3,

				[Description("Four")]
				Fourth = 4,

				Fifth = 5
			}

			[Theory]
			[InlineData("one", MyEnum.First)]
			[InlineData("two", MyEnum.Second)]
			[InlineData("second", MyEnum.Second)]
			[InlineData("alternative", MyEnum.Second)]
			[InlineData("three", MyEnum.Third)]
			[InlineData("four", MyEnum.Fourth)]
			[InlineData("fifth", MyEnum.Fifth)]
			public void ToEnum(string value, MyEnum expected)
			{
				// Act
				var result = value.ToEnum<MyEnum>();

				// Assert
				result.ShouldBe(expected);
			}

			[Theory]
			[InlineData(MyEnum.First, "One")]
			[InlineData(MyEnum.Second, "Two")]
			[InlineData(MyEnum.Third, "Three")]
			[InlineData(MyEnum.Fourth, "Four")]
			[InlineData(MyEnum.Fifth, "Fifth")]
			public void ToEnumString(MyEnum value, string expected)
			{
				// Act
				var result = value.ToEnumString();

				// Assert
				result.ShouldBe(expected);

			}

			[Fact]
			public void ThrowsWhenUndefinedValue()
			{
				// Arrange
				var myInvalidEnumValue = (MyEnum)9999;

				// Act
				Should.Throw<ArgumentException>(() => myInvalidEnumValue.TryToEnumString(out string stringValue, true));
			}

			[Fact]
			public void UndefinedValueCanBeIgnored()
			{
				// Arrange
				var myInvalidEnumValue = (MyEnum)9999;

				// Act
				var result = myInvalidEnumValue.TryToEnumString(out string stringValue, false);

				// Asert
				result.ShouldBeFalse();
				stringValue.ShouldBeNull();
			}
		}

		public class GetErrorMessageAsync
		{
			[Fact]
			public async Task CanHandleUnescapedDoubleQuotesInErrorMessage()
			{
				// Arrange
				const string responseContent = @"{""code"":104, ""message"":""Invalid access token, does not contain scopes:[""zoom_events_basic:read"",""zoom_events_basic:read:admin""]""}";
				var message = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseContent) };
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				var (isError, errorMessage, errorCode) = await response.Message.GetErrorMessageAsync();

				// Assert
				isError.ShouldBeTrue();
				errorMessage.ShouldStartWith("Invalid access token, does not contain scopes");
				errorCode.ShouldBe(104);
			}

			[Fact]
			public async Task IncludesFieldNameInErrorMessage()
			{
				// Arrange
				const string responseContent = @"{""code"":300,""message"":""Validation Failed."",""errors"":[{""field"":""settings.jbh_time"",""message"":""Invalid parameter: jbh_time.""}]}";
				var message = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseContent) };
				var response = new MockFluentHttpResponse(message, null, TestContext.Current.CancellationToken);

				// Act
				var (isError, errorMessage, errorCode) = await response.Message.GetErrorMessageAsync();

				// Assert
				isError.ShouldBeTrue();
				errorMessage.ShouldBe("Validation Failed. settings.jbh_time Invalid parameter: jbh_time.");
				errorCode.ShouldBe(300);
			}
		}
	}
}
