using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Extensions
{
	public class InternalTests
	{
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
				var response = new MockFluentHttpResponse(message, null, CancellationToken.None);

				// Act
				await Should.ThrowAsync<ArgumentException>(() => response.AsPaginatedResponse<UserCallLog>("call_logs")).ConfigureAwait(false);
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
				var response = new MockFluentHttpResponse(message, null, CancellationToken.None);

				// Act
				var paginatedResponse = await response.AsPaginatedResponse<UserCallLog>("call_logs").ConfigureAwait(false);

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
				var response = new MockFluentHttpResponse(message, null, CancellationToken.None);

				// Act
				await Should.ThrowAsync<ArgumentException>(() => response.AsPaginatedResponseWithToken<UserCallLog>("call_logs")).ConfigureAwait(false);
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
				var response = new MockFluentHttpResponse(message, null, CancellationToken.None);

				// Act
				var paginatedResponse = await response.AsPaginatedResponseWithToken<UserCallLog>("call_logs").ConfigureAwait(false);

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
				var response = new MockFluentHttpResponse(message, null, CancellationToken.None);

				// Act
				await Should.ThrowAsync<ArgumentException>(() => response.AsPaginatedResponseWithTokenAndDateRange<UserCallLog>("call_logs")).ConfigureAwait(false);
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
				var response = new MockFluentHttpResponse(message, null, CancellationToken.None);

				// Act
				var paginatedResponse = await response.AsPaginatedResponseWithTokenAndDateRange<UserCallLog>("call_logs").ConfigureAwait(false);

				// Assert
				paginatedResponse.PageSize.ShouldBe(100);
				paginatedResponse.Records.ShouldBeEmpty();
				paginatedResponse.TotalRecords.ShouldBe(0);
			}
		}
	}
}
