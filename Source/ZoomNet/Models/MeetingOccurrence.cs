using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Meeting occurrence.</summary>
	public class MeetingOccurrence
	{
		/// <summary>Gets or sets the duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public int? Duration { get; set; }

		/// <summary>Gets or sets the occurrence Id.</summary>
		[JsonPropertyName("occurrence_id")]
		public string OccurrenceId { get; set; }

		/// <summary>Gets or sets the start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>Gets or sets the status.</summary>
		[JsonPropertyName("status")]
		public string Status { get; set; }
	}
}
