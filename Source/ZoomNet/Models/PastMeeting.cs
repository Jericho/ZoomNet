using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A meeting that occurred in the past.</summary>
	public class PastMeeting : MeetingBasicInfo
	{
		/// <summary>Gets or sets the meeting host's department.</summary>
		[JsonPropertyName("dept")]
		public string Department { get; set; }

		/// <summary>Gets or sets the meeting duration in minutes.</summary>
		[JsonPropertyName("duration")]
		public long Duration { get; set; }

		/// <summary>Gets or sets the date and time when the meeting ended.</summary>
		[JsonPropertyName("end_time")]
		public DateTime EndedOn { get; set; }

		/// <summary>Gets or sets the number of participants.</summary>
		[JsonPropertyName("participants_count")]
		public long ParticipantsCount { get; set; }

		/// <summary>Gets or sets the value indicating whether the meeting was created directly through Zoom or via an API request.</summary>
		/// <remarks>
		/// If the meeting was created via an OAuth app, this field returns the OAuth app's name.
		/// If the meeting was created via JWT or the Zoom Web Portal, this returns the Zoom value.
		/// </remarks>
		[JsonPropertyName("source")]
		public string Source { get; set; }

		/// <summary>Gets or sets the date and time when the meeting started.</summary>
		[JsonPropertyName("start_time")]
		public DateTime StartedOn { get; set; }

		/// <summary>Gets or sets the sum of meeting minutes from all participants.</summary>
		[JsonPropertyName("total_minutes")]
		public long TotalMinutes { get; set; }

		/// <summary>Gets or sets the meeting type.</summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>Gets or sets the user email.</summary>
		[JsonPropertyName("user_email")]
		public string UserEmail { get; set; }

		/// <summary>Gets or sets the user display name.</summary>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }
	}
}
