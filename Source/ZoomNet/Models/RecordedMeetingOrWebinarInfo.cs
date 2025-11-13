using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents information about recorded meeting or webinar.
	/// </summary>
	public class RecordedMeetingOrWebinarInfo
	{
		/// <summary>
		/// Gets or sets the id of the meeting or webinar.
		/// </summary>
		[JsonPropertyName("id")]
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the uuid of the meeting or webinar.
		/// </summary>
		[JsonPropertyName("uuid")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the id of the user set as the host of the meeting or webinar.
		/// </summary>
		[JsonPropertyName("host_id")]
		public string HostId { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar topic.
		/// </summary>
		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		/// <summary>
		/// Gets or sets the type of the meeting or webinar.
		/// </summary>
		[JsonPropertyName("type")]
		public RecordingType Type { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar start time.
		/// </summary>
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar timezone.
		/// </summary>
		[JsonPropertyName("timezone")]
		public TimeZones Timezone { get; set; }

		/// <summary>
		/// Gets or sets the meeting or webinar scheduled duration in minutes.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }
	}
}
