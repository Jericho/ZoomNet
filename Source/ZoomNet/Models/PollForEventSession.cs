using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A poll associated with an event session.</summary>
	public class PollForEventSession : Poll
	{
		/// <summary>Gets or sets the status of the poll.</summary>
		[JsonPropertyName("status")]
		public PollStatusForEventSession Status { get; set; }

		/// <summary>Gets or sets the questions.</summary>
		[JsonPropertyName("questions")]
		public PollQuestionForEventSession[] Questions { get; set; }
	}
}
