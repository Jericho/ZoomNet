using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents meeting and transcript file information as received in <see cref="Webhooks.MeetingAicTranscriptCompletedEvent"/>.
	/// </summary>
	public class WebhookAiCompanionMeetingTranscript
	{
		/// <summary>
		/// Gets or sets the meeting id.
		/// </summary>
		[JsonPropertyName("meeting_id")]
		public long Id { get;set; }

		/// <summary>
		/// Gets or sets the unique meeting id.
		/// </summary>
		[JsonPropertyName("meeting_uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the id of the user who is set as the meeting host.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the meeting topic.
		/// </summary>
		[JsonPropertyName("meeting_topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the meeting's start date and time.
		/// </summary>
		[JsonPropertyName("meeting_start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the transcript file id.
		/// </summary>
		[JsonPropertyName("file_id")]
		public string FileId { get; set; }

		/// <summary>
		/// Gets or sets attachment type.
		/// </summary>
		/// <remarks>
		/// Equals 'durable_transcript' in <see cref="Webhooks.MeetingAicTranscriptCompletedEvent"/>.
		/// </remarks>
		[JsonPropertyName("attach_type")]
		public string AttachType { get; set; }
	}
}
