using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics for a meeting.
	/// </summary>
	public class DashboardMeetingMetrics : DashboardMetricsBase
	{
		/// <summary>
		/// Gets or sets the number of Zoom Room participants in the meeting.
		/// </summary>
		/// <value>
		/// The number of Zoom Room participants in the meeting.
		/// </value>
		[JsonPropertyName("in_room_participants")]
		public int InRoomParticipants { get; set; }
	}
}
