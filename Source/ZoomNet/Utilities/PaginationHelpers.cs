using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Provides extension methods for paginated Zoom API responses to enable easier consumption.
	/// </summary>
	public static class PaginationHelpers
	{
		/// <summary>
		/// Enumerates all records from a paginated API endpoint that uses page tokens.
		/// </summary>
		/// <typeparam name="T">The type of records in the paginated response.</typeparam>
		/// <param name="getFirstPage">A function to retrieve the first page of results.</param>
		/// <param name="getNextPage">A function to retrieve subsequent pages using a paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of all records across all pages.</returns>
		/// <example>
		/// <code>
		/// await foreach (var meeting in client.Meetings
		///     .GetAllAsync(userId, 100)
		///     .EnumerateAllRecordsAsync(cancellationToken))
		/// {
		///     Console.WriteLine(meeting.Topic);
		/// }
		/// </code>
		/// </example>
		public static async IAsyncEnumerable<T> EnumerateAllRecordsAsync<T>(
			Func<Task<PaginatedResponseWithToken<T>>> getFirstPage,
			Func<string, Task<PaginatedResponseWithToken<T>>> getNextPage,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			// Get the first page
			var response = await getFirstPage().ConfigureAwait(false);

			// Yield records from the first page
			foreach (var record in response.Records)
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return record;
			}

			// Continue fetching pages while there's a next page token
			while (response.MoreRecordsAvailable)
			{
				cancellationToken.ThrowIfCancellationRequested();

				// Fetch the next page
				response = await getNextPage(response.NextPageToken).ConfigureAwait(false);

				// Yield records from this page
				foreach (var record in response.Records)
				{
					cancellationToken.ThrowIfCancellationRequested();
					yield return record;
				}
			}
		}

		/// <summary>
		/// Enumerates all records from a paginated API endpoint that uses page tokens and includes a date range.
		/// </summary>
		/// <typeparam name="T">The type of records in the paginated response.</typeparam>
		/// <param name="getFirstPage">A function to retrieve the first page of results.</param>
		/// <param name="getNextPage">A function to retrieve subsequent pages using a paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An async enumerable of all records across all pages.</returns>
		public static async IAsyncEnumerable<T> EnumerateAllRecordsAsync<T>(
			Func<Task<PaginatedResponseWithTokenAndDateRange<T>>> getFirstPage,
			Func<string, Task<PaginatedResponseWithTokenAndDateRange<T>>> getNextPage,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			// Get the first page
			var response = await getFirstPage().ConfigureAwait(false);

			// Yield records from the first page
			foreach (var record in response.Records)
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return record;
			}

			// Continue fetching pages while there's a next page token
			while (response.MoreRecordsAvailable)
			{
				cancellationToken.ThrowIfCancellationRequested();

				// Fetch the next page
				response = await getNextPage(response.NextPageToken).ConfigureAwait(false);

				// Yield records from this page
				foreach (var record in response.Records)
				{
					cancellationToken.ThrowIfCancellationRequested();
					yield return record;
				}
			}
		}
	}
}
