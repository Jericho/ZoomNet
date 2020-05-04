using Newtonsoft.Json;

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
		[JsonProperty("question", NullValueHandling = NullValueHandling.Ignore)]
		public string Question { get; set; }

		/// <summary>
		/// Gets or sets the answer submitted by the participant.
		/// </summary>
		/// <value>
		/// The question.
		/// </value>
		[JsonProperty("answer", NullValueHandling = NullValueHandling.Ignore)]
		public string Answer { get; set; }
	}
}
