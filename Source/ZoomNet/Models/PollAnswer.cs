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

		/// <summary>
		/// Gets or sets the unique identifier of the poll.
		/// </summary>
		[JsonPropertyName("polling_id")]
		public string PollId { get; set; }

		/// <summary>
		/// Gets or sets the date and time at which the answer to the poll was submitted.
		/// </summary>
		[JsonPropertyName("date_time")]
		public string SubmittedOn { get; set; }
	}
}
