using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Registrant info.
	/// </summary>
	public class RegistrantInfo
	{
		/// <summary>
		/// Gets or sets the registrant id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("registrant_id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the meeting or webinar.
		/// </summary>
		[JsonPropertyName("id")]
		public long EventId { get; set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the URL for this registrant to join the meeting or webinar.
		/// </summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }
	}
}
