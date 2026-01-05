using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Utilities
{
	public class PaginationExtensionsTests
	{
		#region EnumerateAllRecordsAsync Tests

		[Fact]
		public async Task EnumerateAllRecordsAsync_WithSinglePage_YieldsAllRecords()
		{
			// Arrange
			var records = new[] { "Item1", "Item2", "Item3" };
			var page1 = new PaginatedResponseWithToken<string>
			{
				Records = records,
				NextPageToken = null,
				RecordsPerPage = 3
			};

			var getFirstPage = new Func<Task<PaginatedResponseWithToken<string>>>(() => Task.FromResult(page1));
			var getNextPage = new Func<string, Task<PaginatedResponseWithToken<string>>>(token => throw new InvalidOperationException("Should not be called"));

			// Act
			var result = new List<string>();
			await foreach (var record in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, CancellationToken.None))
			{
				result.Add(record);
			}

			// Assert
			result.Count.ShouldBe(3);
			result.ShouldBe(records);
		}

		[Fact]
		public async Task EnumerateAllRecordsAsync_WithMultiplePages_YieldsAllRecords()
		{
			// Arrange
			var page1 = new PaginatedResponseWithToken<string>
			{
				Records = new[] { "Item1", "Item2" },
				NextPageToken = "token1",
				RecordsPerPage = 2
			};

			var page2 = new PaginatedResponseWithToken<string>
			{
				Records = new[] { "Item3", "Item4" },
				NextPageToken = "token2",
				RecordsPerPage = 2
			};

			var page3 = new PaginatedResponseWithToken<string>
			{
				Records = new[] { "Item5" },
				NextPageToken = null,
				RecordsPerPage = 2
			};

			var callCount = 0;
			var getFirstPage = new Func<Task<PaginatedResponseWithToken<string>>>(() => Task.FromResult(page1));
			var getNextPage = new Func<string, Task<PaginatedResponseWithToken<string>>>(token =>
			{
				callCount++;
				if (token == "token1") return Task.FromResult(page2);
				if (token == "token2") return Task.FromResult(page3);
				throw new InvalidOperationException($"Unexpected token: {token}");
			});

			// Act
			var result = new List<string>();
			await foreach (var record in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, CancellationToken.None))
			{
				result.Add(record);
			}

			// Assert
			result.Count.ShouldBe(5);
			result.ShouldBe(["Item1", "Item2", "Item3", "Item4", "Item5"]);
			callCount.ShouldBe(2);
		}

		[Fact]
		public async Task EnumerateAllRecordsAsync_WithEmptyFirstPage_YieldsNothing()
		{
			// Arrange
			var page1 = new PaginatedResponseWithToken<string>
			{
				Records = Array.Empty<string>(),
				NextPageToken = null,
				RecordsPerPage = 0
			};

			var getFirstPage = new Func<Task<PaginatedResponseWithToken<string>>>(() => Task.FromResult(page1));
			var getNextPage = new Func<string, Task<PaginatedResponseWithToken<string>>>(token => throw new InvalidOperationException("Should not be called"));

			// Act
			var result = new List<string>();
			await foreach (var record in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, CancellationToken.None))
			{
				result.Add(record);
			}

			// Assert
			result.Count.ShouldBe(0);
		}

		[Fact]
		public async Task EnumerateAllRecordsAsync_WithCancellation_ThrowsOperationCanceledException()
		{
			// Arrange
			var cts = new CancellationTokenSource();
			var page1 = new PaginatedResponseWithToken<string>
			{
				Records = new[] { "Item1", "Item2" },
				NextPageToken = "token1",
				RecordsPerPage = 2
			};

			var getFirstPage = new Func<Task<PaginatedResponseWithToken<string>>>(() => Task.FromResult(page1));
			var getNextPage = new Func<string, Task<PaginatedResponseWithToken<string>>>(token =>
			{
				cts.Cancel();
				return Task.FromResult(new PaginatedResponseWithToken<string>
				{
					Records = ["Item3"],
					NextPageToken = null,
					RecordsPerPage = 1
				});
			});

			// Act & Assert
			await Should.ThrowAsync<OperationCanceledException>(async () =>
			{
				await foreach (var record in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, cts.Token))
				{
					// Process records
				}
			});
		}

		[Fact]
		public async Task EnumerateAllRecordsAsync_WithDateRange_YieldsAllRecords()
		{
			// Arrange
			var page1 = new PaginatedResponseWithTokenAndDateRange<string>
			{
				Records = new[] { "Item1", "Item2" },
				NextPageToken = "token1",
				RecordsPerPage = 2,
				From = DateTime.Parse("2024-01-01"),
				To = DateTime.Parse("2024-01-31")
			};

			var page2 = new PaginatedResponseWithTokenAndDateRange<string>
			{
				Records = new[] { "Item3" },
				NextPageToken = null,
				RecordsPerPage = 2,
				From = DateTime.Parse("2024-01-01"),
				To = DateTime.Parse("2024-01-31")
			};

			var getFirstPage = new Func<Task<PaginatedResponseWithTokenAndDateRange<string>>>(() => Task.FromResult(page1));
			var getNextPage = new Func<string, Task<PaginatedResponseWithTokenAndDateRange<string>>>(token =>
			{
				if (token == "token1") return Task.FromResult(page2);
				throw new InvalidOperationException($"Unexpected token: {token}");
			});

			// Act
			var result = new List<string>();
			await foreach (var record in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, CancellationToken.None))
			{
				result.Add(record);
			}

			// Assert
			result.Count.ShouldBe(3);
			result.ShouldBe(["Item1", "Item2", "Item3"]);
		}

		#endregion

		#region Real-World Scenario Tests

		[Fact]
		public async Task EnumerateAllRecordsAsync_LargeDataset_HandlesCorrectly()
		{
			// Arrange - Simulate 1000 records across 10 pages
			var pages = Enumerable.Range(0, 10).Select(i => new PaginatedResponseWithToken<int>
			{
				Records = Enumerable.Range(i * 100, 100).ToArray(),
				NextPageToken = i < 9 ? $"token{i + 1}" : null,
				RecordsPerPage = 100
			}).ToList();

			var currentPageIndex = 0;
			var getFirstPage = new Func<Task<PaginatedResponseWithToken<int>>>(() => Task.FromResult(pages[currentPageIndex++]));
			var getNextPage = new Func<string, Task<PaginatedResponseWithToken<int>>>(token => Task.FromResult(pages[currentPageIndex++]));

			// Act
			var result = new List<int>();
			await foreach (var record in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, CancellationToken.None))
			{
				result.Add(record);
			}

			// Assert
			result.Count.ShouldBe(1000);
			result.First().ShouldBe(0);
			result.Last().ShouldBe(999);
		}

		[Fact]
		public async Task EnumerateAllRecordsAsync_WithModelObjects_WorksCorrectly()
		{
			// Arrange
			var page1 = new PaginatedResponseWithToken<MeetingSummary>
			{
				Records = new[]
				{
					new MeetingSummary { Id = 1, Topic = "Meeting 1" },
					new MeetingSummary { Id = 2, Topic = "Meeting 2" }
				},
				NextPageToken = "token1",
				RecordsPerPage = 2
			};

			var page2 = new PaginatedResponseWithToken<MeetingSummary>
			{
				Records = new[]
				{
					new MeetingSummary { Id = 3, Topic = "Meeting 3" }
				},
				NextPageToken = null,
				RecordsPerPage = 2
			};

			var getFirstPage = new Func<Task<PaginatedResponseWithToken<MeetingSummary>>>(() => Task.FromResult(page1));
			var getNextPage = new Func<string, Task<PaginatedResponseWithToken<MeetingSummary>>>(token =>
			{
				if (token == "token1") return Task.FromResult(page2);
				throw new InvalidOperationException($"Unexpected token: {token}");
			});

			// Act
			var result = new List<MeetingSummary>();
			await foreach (var meeting in ZoomNet.Utilities.PaginationHelpers.EnumerateAllRecordsAsync(getFirstPage, getNextPage, CancellationToken.None))
			{
				result.Add(meeting);
			}

			// Assert
			result.Count.ShouldBe(3);
			result[0].Topic.ShouldBe("Meeting 1");
			result[1].Topic.ShouldBe("Meeting 2");
			result[2].Topic.ShouldBe("Meeting 3");
		}

		#endregion
	}
}
