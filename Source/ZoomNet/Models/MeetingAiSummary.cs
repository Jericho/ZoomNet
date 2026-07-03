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
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the summary's end date and time.
		/// </summary>
		[JsonPropertyName("summary_end_time")]
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting summary was created.
		/// </summary>
		[JsonPropertyName("summary_created_time")]
		public DateTime CreatedTime { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting summary was last modified.
		/// </summary>
		[JsonPropertyName("summary_last_modified_time")]
		public DateTime LastModifiedTime { get; set; }

		/// <summary>
		/// Gets or sets the user ID of the user who last modified the meeting summary.
		/// </summary>
		[JsonPropertyName("summary_last_modified_user_id")]
		public string LastModifiedUserId { get; set; }

		/// <summary>
		/// Gets or sets the user email of the user who last modified the meeting summary.
		/// </summary>
		[JsonPropertyName("summary_last_modified_user_email")]
		public string LastModifiedUserEmail { get; set; }

		/// <summary>
		/// Gets or sets the summary title.
		/// </summary>
		[JsonPropertyName("summary_title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the complete meeting summary in Markdown format.
		/// </summary>
		[JsonPropertyName("summary_content")]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the URL to view the full summary document in Zoom Docs.
		/// </summary>
		[JsonPropertyName("summary_doc_url")]
		public string DocUrl { get; set; }

		/// <summary>
		/// Gets or sets the summary overview.
		/// </summary>
		[JsonPropertyName("summary_overview")]
		public string Overview { get; set; }

		/// <summary>
		/// Gets or sets the summary details sections.
		/// </summary>
		[JsonPropertyName("summary_details")]
		public MeetingAiSummarySection[] Details { get; set; }

		/// <summary>
		/// Gets or sets the next steps.
		/// </summary>
		[JsonPropertyName("next_steps")]
		public string[] NextSteps { get; set; }

		/// <summary>
		/// Gets or sets the edited summary.
		/// </summary>
		[JsonPropertyName("edited_summary")]
		public MeetingAiEditedSummary EditedSummary { get; set; }
	}
}
