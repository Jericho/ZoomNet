using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about a meeting or webinar transcript.
	/// </summary>
	public class TranscriptInfo
	{
		/// <summary>Gets or sets the user account unique identifier.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>Gets or sets a value indicating whether the meeting transcript will be automatically deleted after a specified number of days.</summary>
		/// <remarks>The host of the recording must have the recording setting Delete cloud recordings after a specified number of days enabled.</remarks>
		[JsonPropertyName("auto_delete")]
		public bool AutoDelete { get; set; }

		/// <summary>Gets or sets the date when the recording will be auto-deleted when auto_delete is true. Otherwise, no date will be returned.</summary>
		[JsonPropertyName("auto_delete_date")]
		public DateTime? AutoDeleteDate { get; set; }

		/// <summary>Gets or sets a value indicating whether the meeting transcript is available for download.
		/// - true: The transcript is ready and download_url will be returned.
		/// - false: The transcript cannot be downloaded and the download_restriction_reason field will be returned instead with the explanation.
		/// Only when can_download is true, the transcript file can be accessed.</summary>
		[JsonPropertyName("can_download")]
		public bool CanDownload { get; set; }

		/// <summary>
		/// Gets or sets the reason why the transcript cannot be downloaded.
		/// </summary>
		[JsonPropertyName("download_restriction_reason")]
		public TranscriptRestrictionReason DownloadRestrictionReason { get; set; }

		/// <summary>Gets or sets the URL to download the transcript.</summary>
		/// <remarks>
		/// This field is only present when can_download is true.
		/// If a user has authorized and installed your OAuth app that contains recording scopes, use the user's OAuth access token to download the file.
		/// Set the access_token as a Bearer token in the Authorization header. For example: curl -H 'Authorization: Bearer ACCESS_TOKEN' https://{{base-domain}}/rec/archive/download/xyz.
		/// </remarks>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; set; }

		/// <summary>Gets or sets the ID of the user set as the host of the meeting.</summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>Gets or sets the meeting ID.</summary>
		/// <remarks>Meeting Ids (and webinar Ids as well) are typically defined as numerical in the ZoomNet library but in this case we defined it as a string becuase it can contain either the Id (which is a number) or the UUID (which is a string).</remarks>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; set; }

		/// <summary>Gets or sets the meeintg topic.</summary>
		[JsonPropertyName("meeting_topic")]
		public string Topic { get; set; }

		/// <summary>Gets or sets the date and time that the transcript was created.</summary>
		[JsonPropertyName("transcript_created_time")]
		public DateTime? TranscriptCreatedOn { get; set; }
	}
}
