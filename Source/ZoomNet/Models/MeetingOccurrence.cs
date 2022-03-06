using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting occurrence.
	/// </summary>
	public class MeetingOccurrence
	{
		/// <summary>
		/// Gets or sets the occurrence Id.
		/// </summary>
		/// <value>The ID.</value>
		[JsonPropertyName("occurrence_id")]
		public string OccurrenceId { get; set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		/// <value>The occurrence start time.</value>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		/// <value>The duration in minutes.</value>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The occurrence status.
		/// </value>
		[JsonPropertyName("status")]
		public string Status { get; set; }
	}
}
