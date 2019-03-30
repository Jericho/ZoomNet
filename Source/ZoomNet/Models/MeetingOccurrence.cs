using Newtonsoft.Json;
using System;

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
		[JsonProperty(PropertyName = "occurrence_id")]
		public string OccurrenceId { get; set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		/// <value>The occurrence start time.</value>
		[JsonProperty(PropertyName = "start_time", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		/// <value>The duration in minutes.</value>
		[JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The occurrence status.
		/// </value>
		[JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
		public string Status { get; set; }
	}
}
