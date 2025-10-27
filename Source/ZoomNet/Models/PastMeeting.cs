using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting that occurred in the past.
	/// </summary>
	public class PastMeeting : MeetingBasicInfo
	{
		/// <summary>
		/// Gets or sets the meeting duration in minutes.
		/// </summary>
		/// <value>The meeting duration.</value>
		[JsonPropertyName("duration")]
		public long Duration { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting started.
		/// </summary>
		/// <value>The meeting start time.</value>
		[JsonPropertyName("start_time")]
		public DateTime StartedOn { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting ended.
		/// </summary>
		/// <value>The meeting end time.</value>
		[JsonPropertyName("end_time")]
		public DateTime EndedOn { get; set; }

		/// <summary>
		/// Gets or sets the meeting host's department.
		/// </summary>
		/// <value>
		/// The department.
		/// </value>
		[JsonPropertyName("dept")]
		public string Department { get; set; }

		/// <summary>
		/// Gets or sets the number of participants.
		/// </summary>
		/// <value>The number of participants.</value>
		[JsonPropertyName("participants_count")]
		public long ParticipantsCount { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether the meeting was created directly through Zoom or via an API request.
		/// </summary>
		/// <remarks>
		/// If the meeting was created via an OAuth app, this field returns the OAuth app's name.
		/// If the meeting was created via JWT or the Zoom Web Portal, this returns the Zoom value.
		/// </remarks>
		/// <value>
		/// The source.
		/// </value>
		[JsonPropertyName("source")]
		public string Source { get; set; }

		/// <summary>
		/// Gets or sets the sum of meeting minutes from all participants.
		/// </summary>
		/// <value>The total meeting minutes.</value>
		[JsonPropertyName("total_minutes")]
		public long TotalMinutes { get; set; }

		/// <summary>
		/// Gets or sets the meeting type.
		/// </summary>
		/// <value>The meeting type.</value>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>
		/// Gets or sets the user email.
		/// </summary>
		/// <value>The user email.</value>
		[JsonPropertyName("user_email")]
		public string UserEmail { get; set; }

		/// <summary>
		/// Gets or sets the user display name.
		/// </summary>
		/// <value>The user display name.</value>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }
	}
}
