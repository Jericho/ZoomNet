using System;

namespace ZoomNet.Models
{
	/// <summary>Pagination Object.</summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public record PaginatedResponseWithTokenAndDateRange<T> : PaginatedResponseWithToken<T>
	{
		/// <summary>Gets or sets the start date for the report.</summary>
		public DateOnly From { get; init; }

		/// <summary>Gets or sets the end date for the report.</summary>
		public DateOnly To { get; init; }
	}
}
