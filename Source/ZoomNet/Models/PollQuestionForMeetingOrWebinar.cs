using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The answer to a question asked during a meeting/webinar poll.
	/// </summary>
	public class PollQuestionForMeetingOrWebinar : PollQuestion
	{
		/// <summary>Gets or sets the information about the prompt questions.</summary>
		/// <remarks>
		/// This field only applies to questions of type 'Matching' and 'Rank'.
		/// You must provide at least two prompts and no more than 10 prompts.
		/// </remarks>
		[JsonPropertyName("prompts")]
		public PollPrompt[] Prompts { get; set; }
	}
}
