using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Meeting transcript metadata returned by the Zoom Meetings API.</summary>
	public record MeetingTranscriptInfo
	{
		/// <summary>Gets or sets the account ID.</summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; init; }

		/// <summary>Gets or sets a value indicating whether the transcript will be automatically deleted.</summary>
		[JsonPropertyName("auto_delete")]
		public bool AutoDelete { get; init; }

		/// <summary>Gets or sets the date when the transcript will be automatically deleted.</summary>
		[JsonPropertyName("auto_delete_date")]
		public string AutoDeleteDate { get; init; }

		/// <summary>Gets or sets a value indicating whether the transcript can be downloaded.</summary>
		[JsonPropertyName("can_download")]
		public bool CanDownload { get; init; }

		/// <summary>Gets or sets the reason why the transcript cannot be downloaded, if applicable.</summary>
		[JsonPropertyName("download_restriction_reason")]
		public TranscriptDownloadRestrictionReason DownloadRestrictionReason { get; init; }

		/// <summary>Gets or sets the URL to download the transcript.</summary>
		[JsonPropertyName("download_url")]
		public string DownloadUrl { get; init; }

		/// <summary>Gets or sets the ID of the user who is set as the meeting host.</summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; init; }

		/// <summary>Gets or sets the unique meeting UUID.</summary>
		[JsonPropertyName("meeting_id")]
		public string MeetingId { get; init; }

		/// <summary>Gets or sets the meeting topic.</summary>
		[JsonPropertyName("meeting_topic")]
		public string Topic { get; init; }

		/// <summary>Gets or sets the date and time the transcript was created.</summary>
		[JsonPropertyName("transcript_created_time")]
		public DateTime CreatedTime { get; init; }
	}
}
