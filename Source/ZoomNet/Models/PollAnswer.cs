using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// The answer to a question asked during a poll.
	/// </summary>
	public class PollAnswer
	{
		/// <summary>
		/// Gets or sets the question asked during the poll.
		/// </summary>
		/// <value>
		/// The question.
		/// </value>
		[JsonPropertyName("question")]
		public string Question { get; set; }

		/// <summary>
		/// Gets or sets the answer submitted by the participant.
		/// </summary>
		/// <value>
		/// The question.
		/// </value>
		[JsonPropertyName("answer")]
		public string Answer { get; set; }
	}
}
