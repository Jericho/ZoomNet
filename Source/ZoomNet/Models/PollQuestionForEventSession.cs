using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The answer to a question asked during am event session poll.
	/// </summary>
	public class PollQuestionForEventSession : PollQuestion
	{
		/// <summary>Gets or sets the information about the prompt questions.</summary>
		/// <remarks>
		/// This field only applies to questions of type 'Matching' and 'Rank'.
		/// You must provide a minimum of two prompts, up to a maximum of 10 prompts.
		/// </remarks>
		[JsonPropertyName("prompts")]
		public string[] Prompts { get; set; }
	}
}
