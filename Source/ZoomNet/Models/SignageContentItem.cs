using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Content item.</summary>
	public record SignageContentItem
	{
		/// <summary>Gets or sets the content id.</summary>
		[JsonPropertyName("content_id")]
		public string ContentId { get; init; }

		/// <summary>Gets or sets how long the content will be displayed.</summary>
		[JsonPropertyName("duration")]
		public int Duration { get; init; }

		/// <summary>Gets or sets the id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the content.</summary>
		[JsonPropertyName("name")]
		public string Name { get; init; }

		/// <summary>Gets or sets the order of the content in the display.</summary>
		[JsonPropertyName("order")]
		public int Order { get; init; }
	}
}
