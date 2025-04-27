using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Summary information about a meeting.
	/// </summary>
	public class MeetingSummary
	{
		/// <summary>Gets or sets the meeting description.</summary>
		/// <remarks>
		/// The length of agenda gets truncated to 250 characters
		/// when you list all meetings for a user. To view the complete
		/// agenda of a meeting, retrieve details for a single meeting,
		/// use the Get a meeting API.
		/// </remarks>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the meeting.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the meeting id, also known as the meeting number.
		/// </summary>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the URL for the host to start the meeting.
		/// </summary>
		[JsonPropertyName("start_url")]
		public string StartUrl { get; set; }

		/// <summary>
		/// Gets or sets the personal meeting id.
		/// </summary>
		[JsonPropertyName("pmi")]
		public string PersonalMeetingId { get; set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the timezone.
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>
		/// Gets or sets the topic of the meeting.
		/// </summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the meeting type.
		/// </summary>
		[JsonPropertyName("type")]
		public MeetingType Type { get; set; }

		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }
	}
}
