using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents extended information about the meeting.
	/// </summary>
	public class MeetingInfo : MeetingBasicInfo
	{
		/// <summary>
		/// Gets or sets the meeting's type.
		/// </summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>
		/// Gets or sets the meeting's start time.
		/// </summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting's timezone.
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>
		/// Gets or sets the meeting's duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

	}
}
