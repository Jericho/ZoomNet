using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A meeting that occured in the past.
	/// </summary>
	public class PastMeeting
	{
		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		/// <value>
		/// The unique id.
		/// </value>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the meeting id, also known as the meeting number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the meeting.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		/// <value>
		/// The topic.
		/// </value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the meeting type.
		/// </summary>
		/// <value>The meeting type.</value>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>
		/// Gets or sets the user display name.
		/// </summary>
		/// <value>The user display name.</value>
		[JsonPropertyName("user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the user email.
		/// </summary>
		/// <value>The user email.</value>
		[JsonPropertyName("user_email")]
		public string UserEmail { get; set; }

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
		/// Gets or sets the meeting duration in minutes.
		/// </summary>
		/// <value>The meeting duration.</value>
		[JsonPropertyName("duration")]
		public long Duration { get; set; }

		/// <summary>
		/// Gets or sets the sum of meeting minutes from all participants.
		/// </summary>
		/// <value>The total meeting minutes.</value>
		[JsonPropertyName("total_minutes")]
		public long TotalMinutes { get; set; }

		/// <summary>
		/// Gets or sets the number of participants.
		/// </summary>
		/// <value>The number of participants.</value>
		[JsonPropertyName("participants_count")]
		public long ParticipantsCount { get; set; }
	}
}
