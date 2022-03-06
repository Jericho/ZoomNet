using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Pagination Object.
	/// </summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public class PaginatedResponseWithToken<T>
	{
		private int? _totalRecords = null;

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
		public int TotalRecords
		{
			get { return _totalRecords ?? Records?.Length ?? 0; }
			set { _totalRecords = value; }
		}

		/// <summary>
		/// Gets or sets the token to retrieve the next page.
		/// </summary>
		/// <value>The page token.</value>
		/// <remarks>This token expires after 15 minutes.</remarks>
		[JsonPropertyName("next_page_token")]
		public string NextPageToken { get; set; }

		/// <summary>
		/// Gets or sets the records.
		/// </summary>
		/// <value>The records.</value>
		public T[] Records { get; set; }

		/// <summary>
		/// Gets a value indicating whether more records are available.
		/// </summary>
		/// <value>true if more records are available; false otherwise.</value>
		public bool MoreRecordsAvailable => !string.IsNullOrEmpty(NextPageToken);
	}
}
