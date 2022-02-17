using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// The answer to a question asked during a poll.
	/// </summary>
	public class PollQuestion
	{
		/// <summary>
		/// Gets or sets the question asked during the poll.
		/// </summary>
		/// <value>
		/// The question.
		/// </value>
		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
		public string Question { get; set; }

		/// <summary>
		/// Gets or sets the type of question.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
		public PollQuestionType Type { get; set; }

		/// <summary>
		/// Gets or sets the answers to the question.
		/// </summary>
		/// <value>
		/// The answers.
		/// </value>
		[JsonProperty("answer", NullValueHandling = NullValueHandling.Ignore)]
		public string[] Answers { get; set; }
	}
}
