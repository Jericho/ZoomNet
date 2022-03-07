using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Zoom Room.
	/// </summary>
	public class ZoomRoom
	{
		/// <summary>
		/// Gets or sets the room id.
		/// </summary>
		/// <value>
		/// The room id.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the room name.
		/// </summary>
		/// <value>
		/// The room name.
		/// </value>
		[JsonPropertyName("room_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Zoom calendar name.
		/// </summary>
		/// <value>
		/// The Zoom calendar name.
		/// </value>
		[JsonPropertyName("calender_name")]
		public string CalendarName { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room email.
		/// </summary>
		/// <value>
		/// The Zoom room email.
		/// </value>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room email type.
		/// </summary>
		/// <value>
		/// Zoom room email type.
		/// </value>
		[JsonPropertyName("account_type")]
		public string AccountType { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room status.
		/// </summary>
		/// <value>
		/// The Zoom room status.
		/// </value>
		[JsonPropertyName("status")]
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room device IP.
		/// </summary>
		/// <value>
		/// The Zoom room device IP.
		/// </value>
		[JsonPropertyName("device_ip")]
		public string DeviceIp { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room camera.
		/// </summary>
		/// <value>
		/// The Zoom room camera.
		/// </value>
		[JsonPropertyName("camera")]
		public string Camera { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room microphone.
		/// </summary>
		/// <value>
		/// The Zoom room microphone.
		/// </value>
		[JsonPropertyName("microphone")]
		public string Microphone { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room speaker.
		/// </summary>
		/// <value>
		/// The Zoom room speaker.
		/// </value>
		[JsonPropertyName("speaker")]
		public string Speaker { get; set; }

		/// <summary>
		/// Gets or sets the last start time of the Zoom room.
		/// </summary>
		/// <value>
		/// The last start time of the Zoom room.
		/// </value>
		[JsonPropertyName("last_start_time")]
		public string LastStartTime { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room location.
		/// </summary>
		/// <value>
		/// The Zoom room location.
		/// </value>
		[JsonPropertyName("location")]
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the health value.
		/// </summary>
		/// <value>
		/// The health value.
		/// </value>
		[JsonPropertyName("health")]
		public string Health { get; set; }

		/// <summary>
		/// Gets or sets Zoom room issues.
		/// </summary>
		/// <value>
		/// Zoom room issues.
		/// </value>
		[JsonPropertyName("issues")]
		public string[] Issues { get; set; }

		/// <summary>
		/// Gets or sets information on the live meeting in the Zoom room.
		/// </summary>
		/// <value>
		/// Metrics on the currently live meeting if there is one.
		/// </value>
		[JsonPropertyName("live_meeting")]
		public DashboardMeetingMetrics LiveMeeting { get; set; }

		/// <summary>
		/// Gets or sets metrics for previous meetings that happened in this Zoom room.
		/// </summary>
		/// <value>
		/// Pagination object to get metrics on previous meetings that happened in this Zoom room.
		/// </value>
		[JsonPropertyName("past_meetings")]
		public DashboardMeetingMetricsPaginationObject PastMeetings { get; set; }
	}
}
