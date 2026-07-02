using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting AI summary returned by the Zoom Summaries API.
	/// </summary>
	public class MeetingAiSummary
	{
		/// <summary>
		/// Gets or sets the ID of the user who is set as the meeting host.
		/// </summary>
		[JsonPropertyName("meeting_host_id")]
		public string MeetingHostId { get; set; }

		/// <summary>
		/// Gets or sets the meeting host's email address.
		/// </summary>
		[JsonPropertyName("meeting_host_email")]
		public string MeetingHostEmail { get; set; }

		/// <summary>
		/// Gets or sets the unique meeting UUID.
		/// </summary>
		[JsonPropertyName("meeting_uuid")]
		public string MeetingUuid { get; set; }

		/// <summary>
		/// Gets or sets the meeting ID.
		/// </summary>
		[JsonPropertyName("meeting_id")]
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long MeetingId { get; set; }

		/// <summary>
		/// Gets or sets the meeting topic.
		/// </summary>
		[JsonPropertyName("meeting_topic")]
		public string MeetingTopic { get; set; }

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
		/// Gets or sets the date and time when the meeting summary was created.
		/// </summary>
		[JsonPropertyName("summary_created_time")]
		public DateTime SummaryCreatedTime { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting summary was last modified.
		/// </summary>
		[JsonPropertyName("summary_last_modified_time")]
		public DateTime SummaryLastModifiedTime { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user who last modified the meeting summary.
		/// </summary>
		[JsonPropertyName("summary_last_modified_user_id")]
		public string SummaryLastModifiedUserId { get; set; }

		/// <summary>
		/// Gets or sets the user email of the user who last modified the meeting summary.
		/// </summary>
		[JsonPropertyName("summary_last_modified_user_email")]
		public string SummaryLastModifiedUserEmail { get; set; }

		/// <summary>
		/// Gets or sets the summary title.
		/// </summary>
		[JsonPropertyName("summary_title")]
		public string SummaryTitle { get; set; }

		/// <summary>
		/// Gets or sets the complete meeting summary in Markdown format.
		/// </summary>
		[JsonPropertyName("summary_content")]
		public string SummaryContent { get; set; }

		/// <summary>
		/// Gets or sets the URL to view the full summary document in Zoom Docs.
		/// </summary>
		[JsonPropertyName("summary_doc_url")]
		public string SummaryDocUrl { get; set; }

		/// <summary>
		/// Gets or sets the summary overview.
		/// </summary>
		[JsonPropertyName("summary_overview")]
		public string SummaryOverview { get; set; }

		/// <summary>
		/// Gets or sets the summary details sections.
		/// </summary>
		[JsonPropertyName("summary_details")]
		public MeetingAiSummarySection[] SummaryDetails { get; set; }

		/// <summary>
		/// Gets or sets the next steps.
		/// </summary>
		[JsonPropertyName("next_steps")]
		public string[] NextSteps { get; set; }

		/// <summary>
		/// Gets or sets the edited summary.
		/// </summary>
		[JsonPropertyName("edited_summary")]
		public MeetingAiSummaryEdited EditedSummary { get; set; }
	}
}
