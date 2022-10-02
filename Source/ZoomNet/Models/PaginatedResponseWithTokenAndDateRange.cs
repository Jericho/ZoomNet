using System;

namespace ZoomNet.Models
{
	/// <summary>
	/// Pagination Object.
	/// </summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public class PaginatedResponseWithTokenAndDateRange<T> : PaginatedResponseWithToken<T>
	{
		/// <summary>
		/// Gets or sets the start date for the report.
		/// </summary>
		/// <value> Start date for this report.</value>
		public DateTime From { get; set; }

		/// <summary>
		/// Gets or sets the end date for the report.
		/// </summary>
		/// <value> End date for this report.</value>
		public DateTime To { get; set; }
	}
}
