using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Metrics for a meeting.</summary>
	public record DashboardMeetingMetrics : DashboardMetricsBase
	{
		/// <summary>Gets or sets the number of Zoom Room participants in the meeting.</summary>
		[JsonPropertyName("in_room_participants")]
		public int InRoomParticipants { get; init; }
	}
}
