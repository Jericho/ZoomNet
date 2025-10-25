using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting summary information as received	in related webhook events.
	/// </summary>
	public class WebhookMeetingSummary
	{
		/// <summary>
		/// Gets or sets the meeting id.
		/// </summary>
		[JsonPropertyName("meeting_id")]
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long MeetingId { get; set; }

		/// <summary>
		/// Gets or sets the unique meeting id.
		/// </summary>
		[JsonPropertyName("meeting_uuid")]
		public string MeetingUuid { get; set; }

		/// <summary>
		/// Gets or sets the meeting's start date and time.
		/// </summary>
		[JsonPropertyName("meeting_start_time")]
		public DateTime MeetingStartTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting's end date and time.
		/// </summary>
		[JsonPropertyName("meeting_end_time")]
		public DateTime MeetingEndTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting host's email address.
		/// </summary>
		[JsonPropertyName("meeting_host_email")]
		public string MeetingHostEmail { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the meeting host.
		/// </summary>
		[JsonPropertyName("meeting_host_id")]
		public string MeetingHostId { get; set; }

		/// <summary>
		/// Gets or sets meeting topic.
		/// </summary>
		[JsonPropertyName("meeting_topic")]
		public string MeetingTopic { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting summary was created.
		/// </summary>
		[JsonPropertyName("summary_created_time")]
		public DateTime SummaryCreatedTime { get; set; }

		/// <summary>
		/// Gets or sets the summary's start date and time.
		/// </summary>
		[JsonPropertyName("summary_start_time")]
		public DateTime SummaryStartTime { get; set; }

		/// <summary>
		/// Gets or sets the summary's end date and time.
		/// </summary>
		[JsonPropertyName("summary_end_time")]
		public DateTime SummaryEndTime { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting summary was last modified.
		/// </summary>
		[JsonPropertyName("summary_last_modified_time")]
		public DateTime SummaryLastModifiedTime { get; set; }

		/// <summary>
		/// Gets or sets the summary title.
		/// </summary>
		[JsonPropertyName("summary_title")]
		public string SummaryTitle { get; set; }

		/// <summary>
		/// Gets or sets the user email of the user who last modified the meeting summary.
		/// </summary>
		/// <remarks>
		/// Value is available only in <see cref="Webhooks.EventType.MeetingSummaryUpdated"/> webhook.
		/// </remarks>
		[JsonPropertyName("summary_last_modified_user_email")]
		public string SummaryLastModifiedUserEmail { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user who last modified the meeting summary.
		/// </summary>
		/// <remarks>
		/// Value is available only in <see cref="Webhooks.EventType.MeetingSummaryUpdated"/> webhook.
		/// </remarks>
		[JsonPropertyName("summary_last_modified_user_id")]
		public string SummaryLastModifiedUserId { get; set; }

		/// <summary>
		/// Gets or sets the complete meeting summary in Markdown format.
		/// </summary>
		/// <remarks>
		/// Value is available only in <see cref="Webhooks.EventType.MeetingSummaryCompleted"/> and
		/// <see cref="Webhooks.EventType.MeetingSummaryUpdated"/> webhooks.
		/// </remarks>
		[JsonPropertyName("summary_content")]
		public string SummaryContent { get; set; }

		/// <summary>
		/// Gets or sets the URL to view the full summary document in Zoom Docs.
		/// </summary>
		/// <remarks>
		/// Value is available only in <see cref="Webhooks.EventType.MeetingSummaryCompleted"/> and
		/// <see cref="Webhooks.EventType.MeetingSummaryUpdated"/> webhooks.
		/// </remarks>
		[JsonPropertyName("summary_doc_url")]
		public string SummaryDocUrl { get; set; }
	}
}
