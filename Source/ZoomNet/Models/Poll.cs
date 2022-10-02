using System.Text.Json.Serialization;

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
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the status of the poll.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[JsonPropertyName("status")]
		public PollStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the title of the poll.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the type of the poll.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[JsonPropertyName("poll_type")]
		public PollType Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow participants to anonymously answer poll questions.
		/// </summary>
		[JsonPropertyName("anonymous")]
		public bool AllowAnonymous { get; set; }

		/// <summary>
		/// Gets or sets the questions.
		/// </summary>
		/// <value>
		/// The questions.
		/// </value>
		[JsonPropertyName("questions")]
		public PollQuestion[] Questions { get; set; }
	}
}
