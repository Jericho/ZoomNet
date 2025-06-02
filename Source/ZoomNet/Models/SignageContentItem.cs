using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Content item.
	/// </summary>
	public class SignageContentItem
	{
		/// <summary>
		/// Gets or sets the content id.
		/// </summary>
		[JsonPropertyName("content_id")]
		public string ContentId { get; set; }

		/// <summary>
		/// Gets or sets how long the content will be displayed.
		/// </summary>
		[JsonPropertyName("duration")]
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the content.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the order of the content in the display.
		/// </summary>
		[JsonPropertyName("order")]
		public int Order { get; set; }
	}
}
