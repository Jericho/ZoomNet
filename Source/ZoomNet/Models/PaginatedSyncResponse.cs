using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Pagination Object with a sync token.
	/// As far as I know, this is only used when retrieving phone/sms sessions.
	/// </summary>
	/// <typeparam name="T">The type of records.</typeparam>
	public class PaginatedSyncResponse<T>
	{
		/// <summary>Gets or sets the sync token.</summary>
		[JsonPropertyName("sync_token")]
		public string SyncToken { get; set; }

		/// <summary>Gets or sets the records.</summary>
		public T[] Records { get; set; }
	}
}
