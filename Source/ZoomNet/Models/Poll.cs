using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A poll.</summary>
	public abstract class Poll
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the title of the poll.</summary>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>Gets or sets the type of the poll.</summary>
		[JsonPropertyName("poll_type")]
		public PollType Type { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow participants to anonymously answer poll questions.</summary>
		[JsonPropertyName("anonymous")]
		public bool AllowAnonymous { get; set; }
	}
}
