using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Date Object.
	/// </summary>
	public class DailyUsageSummary
	{
		/// <summary>Gets or sets the date.</summary>
		[JsonPropertyName("date")]
		public string Date { get; set; }

		/// <summary>Gets or sets number of meeting minutes.</summary>
		[JsonPropertyName("meeting_minutes")]
		public int MeetingMinutes { get; set; }

		/// <summary>Gets or sets number of meetings.</summary>
		[JsonPropertyName("meetings")]
		public int Meetings { get; set; }

		/// <summary>Gets or sets number of new users.</summary>
		[JsonPropertyName("new_users")]
		public int NewUsers { get; set; }

		/// <summary>Gets or sets number of participants.</summary>
		[JsonPropertyName("participants")]
		public int Participants { get; set; }
	}
}
