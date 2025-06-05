using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Summary information about a webinar.
	/// </summary>
	public class WebinarSummary
	{
		/// <summary>
		/// Gets or sets the webinar agenda.
		/// </summary>
		/// <value>The agenda.</value>
		[JsonPropertyName("agenda")]
		public string Agenda { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the meeting was created.
		/// </summary>
		/// <value>The meeting created time.</value>
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the duration in minutes.
		/// </summary>
		/// <value>The duration in minutes.</value>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who is set as the host of the webinar.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the webinar id, also known as the webinar number.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the webinar.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>
		/// Gets or sets the webinar start time.
		/// </summary>
		/// <value>The webinar start time.</value>
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
		/// <value>
		/// The topic.
		/// </value>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the webinar type.
		/// </summary>
		/// <value>The webinar type.</value>
		[JsonPropertyName("type")]
		public WebinarType Type { get; set; }

		/// <summary>
		/// Gets or sets the unique id.
		/// </summary>
		/// <value>
		/// The unique id.
		/// </value>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }
	}
}
