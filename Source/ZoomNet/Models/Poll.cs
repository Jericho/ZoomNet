using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// A poll.
	/// </summary>
	public class Poll
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The ID.
		/// </value>
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the status of the poll.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
		public PollStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the title of the poll.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the questions.
		/// </summary>
		/// <value>
		/// The questions.
		/// </value>
		[JsonProperty("questions", NullValueHandling = NullValueHandling.Ignore)]
		public PollQuestion[] Questions { get; set; }
	}
}
