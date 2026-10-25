using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Presence status model.</summary>
	public record PresenceStatusResponse
	{
		/// <summary>Gets or sets end time. Visible only in case that the user queries for self and presence status is <see cref="PresenceStatus.DoNotDisturb"/>.</summary>
		[JsonPropertyName("end_time")]
		public DateTime? EndTime { get; init; }

		/// <summary>Gets or sets remaining time. Visible only in case that the user queries for self and presence status is <see cref="PresenceStatus.DoNotDisturb"/>.</summary>
		[JsonPropertyName("remaining_time")]
		public int? RemainingTime { get; init; }

		/// <summary>Gets or sets presence status.</summary>
		[JsonPropertyName("status")]
		public PresenceStatus Status { get; init; }
	}
}
