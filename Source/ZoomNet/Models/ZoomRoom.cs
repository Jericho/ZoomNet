using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the room name.
		/// </summary>
		/// <value>
		/// The room name.
		/// </value>
		[JsonProperty(PropertyName = "room_name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Zoom calendar name.
		/// </summary>
		/// <value>
		/// The Zoom calendar name.
		/// </value>
		[JsonProperty(PropertyName = "calender_name")]
		public string CalendarName { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room email.
		/// </summary>
		/// <value>
		/// The Zoom room email.
		/// </value>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room email type.
		/// </summary>
		/// <value>
		/// Zoom room email type.
		/// </value>
		[JsonProperty(PropertyName = "account_type")]
		public string AccountType { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room status.
		/// </summary>
		/// <value>
		/// The Zoom room status.
		/// </value>
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room device IP.
		/// </summary>
		/// <value>
		/// The Zoom room device IP.
		/// </value>
		[JsonProperty(PropertyName = "device_ip")]
		public string DeviceIp { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room camera.
		/// </summary>
		/// <value>
		/// The Zoom room camera.
		/// </value>
		[JsonProperty(PropertyName = "camera")]
		public string Camera { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room microphone.
		/// </summary>
		/// <value>
		/// The Zoom room microphone.
		/// </value>
		[JsonProperty(PropertyName = "microphone")]
		public string Microphone { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room speaker.
		/// </summary>
		/// <value>
		/// The Zoom room speaker.
		/// </value>
		[JsonProperty(PropertyName = "speaker")]
		public string Speaker { get; set; }

		/// <summary>
		/// Gets or sets the last start time of the Zoom room.
		/// </summary>
		/// <value>
		/// The last start time of the Zoom room.
		/// </value>
		[JsonProperty(PropertyName = "last_start_time")]
		public string LastStartTime { get; set; }

		/// <summary>
		/// Gets or sets the Zoom room location.
		/// </summary>
		/// <value>
		/// The Zoom room location.
		/// </value>
		[JsonProperty(PropertyName = "location")]
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the health value.
		/// </summary>
		/// <value>
		/// The health value.
		/// </value>
		[JsonProperty(PropertyName = "health")]
		public string Health { get; set; }

		/// <summary>
		/// Gets or sets Zoom room issues.
		/// </summary>
		/// <value>
		/// Zoom room issues.
		/// </value>
		[JsonProperty(PropertyName = "issues")]
		public string[] Issues { get; set; }

		/// <summary>
		/// Gets or sets information on the live meeting in the Zoom room.
		/// </summary>
		/// <value>
		/// Metrics on the currently live meeting if there is one.
		/// </value>
		[JsonProperty(PropertyName = "live_meeting", NullValueHandling = NullValueHandling.Ignore)]
		public DashboardMeetingMetrics LiveMeeting { get; set; }

		/// <summary>
		/// Gets or sets metrics for previous meetings that happened in this Zoom room.
		/// </summary>
		/// <value>
		/// Pagination object to get metrics on previous meetings that happened in this Zoom room.
		/// </value>
		[JsonProperty(PropertyName = "past_meetings", NullValueHandling = NullValueHandling.Ignore)]
		public DashboardMeetingMetricsPaginationObject PastMeetings { get; set; }
	}
}
