using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Pagination object for Phone users.
/// </summary>
public class PhoneCallUserProfilesPaginationObject
{
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
	/// Gets or sets the token to retrieve the next page.
	/// </summary>
	/// <value>The page token.</value>
	/// <remarks>This token expires after 15 minutes.</remarks>
	[JsonPropertyName("next_page_token")]
	public string NextPageToken { get; set; }

	/// <summary>
	/// Gets or sets the set of Phone users.
	/// </summary>
	/// <value>The array of Phone call user profiles.</value>
	[JsonPropertyName("users")]
	public PhoneCallUserProfile[] Users { get; set; }
}
