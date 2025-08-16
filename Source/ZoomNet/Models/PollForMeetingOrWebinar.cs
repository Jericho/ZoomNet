using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A poll associated with a meeting or a webinar.</summary>
	public class PollForMeetingOrWebinar : Poll
	{
		/// <summary>Gets or sets the status of the poll.</summary>
		[JsonPropertyName("status")]
		public PollStatusForMeetingOrWebinar Status { get; set; }

		/// <summary>Gets or sets the questions.</summary>
		[JsonPropertyName("questions")]
		public PollQuestionForMeetingOrWebinar[] Questions { get; set; }
	}
}
