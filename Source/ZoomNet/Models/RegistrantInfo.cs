using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Registrant info.</summary>
	public record RegistrantInfo
	{
		/// <summary>Gets or sets the unique identifier of the meeting or webinar.</summary>
		[JsonPropertyName("id")]
		public long EventId { get; init; }

		/// <summary>Gets or sets the URL for this registrant to join the meeting or webinar.</summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; init; }

		/// <summary>Gets or sets the registrant id.</summary>
		[JsonPropertyName("registrant_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the start time.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; init; }
	}
}
