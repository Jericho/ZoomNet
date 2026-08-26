using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Pagination Object.</summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public class PaginatedResponse<T>
	{
		/// <summary>Gets or sets the number of items returned on this page.</summary>
		[JsonPropertyName("page_count")]
		public int PageCount { get; set; }

		/// <summary>Gets or sets the page number of current results.</summary>
		[JsonPropertyName("page_number")]
		public int PageNumber { get; set; }

		/// <summary>Gets or sets the number of records returned within a single API call.</summary>
		[JsonIgnore]
		[Obsolete("Use RecordsPerPage instead.")]
		public int PageSize
		{
			get => RecordsPerPage;
			set => RecordsPerPage = value;
		}

		/// <summary>Gets or sets the number of records returned within a single API call.</summary>
		[JsonPropertyName("page_size")]
		public int RecordsPerPage { get; set; }

		/// <summary>Gets or sets the number of all records available across pages.</summary>
		[JsonPropertyName("total_records")]
		public int? TotalRecords { get; set; }

		/// <summary>Gets or sets the records.</summary>
		public T[] Records { get; set; }
	}
}
