using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Pagination Object.
	/// </summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public class PaginatedResponse<T>
	{
		/// <summary>
		/// Gets or sets the number of items returned on this page.
		/// </summary>
		/// <value>The number of items returned on this page.</value>
		[JsonPropertyName("page_count")]
		public int PageCount { get; set; }

		/// <summary>
		/// Gets or sets the page number of current results.
		/// </summary>
		/// <value>The page number of current results.</value>
		[JsonPropertyName("page_number")]
		public int PageNumber { get; set; }

		/// <summary>
		/// Gets or sets the number of records returned within a single API call.
		/// </summary>
		/// <value>The number of records returned within a single API call.</value>
		[JsonPropertyName("page_size")]
		public int PageSize { get; set; }

		/// <summary>
		/// Gets or sets the number of all records available across pages.
		/// </summary>
		/// <value>The number of all records available across pages.</value>
		[JsonPropertyName("total_records")]
		public int? TotalRecords { get; set; }

		/// <summary>
		/// Gets or sets the records.
		/// </summary>
		/// <value>The records.</value>
		public T[] Records { get; set; }
	}
}
