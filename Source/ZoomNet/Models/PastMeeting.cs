using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A meeting that occurred in the past.</summary>
	public record PastMeeting : MeetingBasicInfo
	{
		/// <summary>Gets or sets the meeting host's department.</summary>
		[JsonPropertyName("dept")]
		public string Department { get; init; }

		/// <summary>Gets or sets the meeting duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public long Duration { get; init; }

		/// <summary>Gets or sets the date and time when the meeting ended.</summary>
		[JsonPropertyName("end_time")]
		public DateTime EndedOn { get; init; }

		/// <summary>Gets or sets the number of participants.</summary>
		[JsonPropertyName("participants_count")]
		public long ParticipantsCount { get; init; }

		/// <summary>Gets or sets the value indicating whether the meeting was created directly through Zoom or via an API request.</summary>
		/// <remarks>
		/// If the meeting was created via an OAuth app, this field returns the OAuth app's name.
		/// If the meeting was created via JWT or the Zoom Web Portal, this returns the Zoom value.
		/// </remarks>
		[JsonPropertyName("source")]
		public string Source { get; init; }

		/// <summary>Gets or sets the date and time when the meeting started.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartedOn { get; init; }

		/// <summary>Gets or sets the sum of meeting minutes from all participants.</summary>
		[JsonPropertyName("total_minutes")]
		public long TotalMinutes { get; init; }

		/// <summary>Gets or sets the meeting type.</summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; init; }

		/// <summary>Gets or sets the user email.</summary>
		[JsonPropertyName("user_email")]
		public string UserEmail { get; init; }

		/// <summary>Gets or sets the user display name.</summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; init; }
	}
}
