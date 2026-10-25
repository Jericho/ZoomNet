using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models
{
	/// <summary>Daily Usage Summary.</summary>
	public record DailyUsageSummary
	{
		/// <summary>Gets or sets the date.</summary>
		[JsonPropertyName("date")]
		[JsonConverter(typeof(DateOnlyConverter))]
		public (int Year, int Month, int Day) Date { get; init; }

		/// <summary>Gets or sets number of meeting minutes.</summary>
		[JsonPropertyName("meeting_minutes")]
		public int MeetingMinutes { get; init; }

		/// <summary>Gets or sets number of meetings.</summary>
		[JsonPropertyName("meetings")]
		public int Meetings { get; init; }

		/// <summary>Gets or sets number of new users.</summary>
		[JsonPropertyName("new_users")]
		public int NewUsers { get; init; }

		/// <summary>Gets or sets number of participants.</summary>
		[JsonPropertyName("participants")]
		public int Participants { get; init; }
	}
}
