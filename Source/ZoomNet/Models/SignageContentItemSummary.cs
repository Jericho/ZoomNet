using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Content item summary.
	/// </summary>
	public class SignageContentItemSummary
	{
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
	}
}
