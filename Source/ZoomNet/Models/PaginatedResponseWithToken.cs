using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Pagination Object.</summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public class PaginatedResponseWithToken<T>
	{
		private int? _totalRecords = null;

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
		public int TotalRecords
		{
			get { return _totalRecords ?? Records?.Length ?? 0; }
			set { _totalRecords = value; }
		}

		/// <summary>Gets or sets the token to retrieve the next page.</summary>
		/// <remarks>This token expires after 15 minutes.</remarks>
		[JsonPropertyName("next_page_token")]
		public string NextPageToken { get; set; }

		/// <summary>Sets the sync token.</summary>
		/// <remarks>
		/// The reason why I added this property on this model is to be able to handle scenarios where the
		/// JSON from Zoom API contains a node called 'sync_token' rather than the expected next_page_token.
		/// One such example is the "Sync user's call history" endpoint, documented here: https://developers.zoom.us/docs/api/phone/#tag/call-logs.
		/// </remarks>
		[JsonPropertyName("sync_token")]
#if NET5_0_OR_GREATER
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
#endif
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string SyncToken
		{
			set { NextPageToken = value; }
		}

		/// <summary>Gets or sets the records.</summary>
		public T[] Records { get; set; }

		/// <summary>Gets a value indicating whether more records are available.</summary>
		public bool MoreRecordsAvailable => !string.IsNullOrEmpty(NextPageToken);
	}
}
