using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Zoom Room.</summary>
	public record ZoomRoom
	{
		/// <summary>Gets or sets the Zoom room email type.</summary>
		[JsonPropertyName("account_type")]
		public string AccountType { get; init; }

		/// <summary>Gets or sets the Zoom calendar name.</summary>
		[JsonPropertyName("calender_name")]
		public string CalendarName { get; init; }

		/// <summary>Gets or sets the Zoom room camera.</summary>
		[JsonPropertyName("camera")]
		public string Camera { get; init; }

		/// <summary>Gets or sets the Zoom room device IP.</summary>
		[JsonPropertyName("device_ip")]
		public string DeviceIp { get; init; }

		/// <summary>Gets or sets the Zoom room email.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the health value.</summary>
		[JsonPropertyName("health")]
		public string Health { get; init; }

		/// <summary>Gets or sets the room id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets Zoom room issues.</summary>
		[JsonPropertyName("issues")]
		public string[] Issues { get; init; }

		/// <summary>Gets or sets the last start time of the Zoom room.</summary>
		[JsonPropertyName("last_start_time")]
		public string LastStartTime { get; init; }

		/// <summary>Gets or sets information on the live meeting in the Zoom room.</summary>
		[JsonPropertyName("live_meeting")]
		public DashboardMeetingMetrics LiveMeeting { get; init; }

		/// <summary>Gets or sets the Zoom room location.</summary>
		[JsonPropertyName("location")]
		public string Location { get; init; }

		/// <summary>Gets or sets the Zoom room microphone.</summary>
		[JsonPropertyName("microphone")]
		public string Microphone { get; init; }

		/// <summary>Gets or sets metrics for previous meetings that happened in this Zoom room.</summary>
		[JsonPropertyName("past_meetings")]
		public DashboardMeetingMetricsPaginationObject PastMeetings { get; init; }

		/// <summary>Gets or sets the room name.</summary>
		[JsonPropertyName("room_name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the Zoom room speaker.</summary>
		[JsonPropertyName("speaker")]
		public string Speaker { get; init; }

		/// <summary>Gets or sets the Zoom room status.</summary>
		[JsonPropertyName("status")]
		public string Status { get; init; }
	}
}
