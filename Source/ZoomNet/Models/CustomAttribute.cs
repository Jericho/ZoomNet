using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Custom attribute.
	/// </summary>
	public class CustomAttribute
	{
		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("key")]
		public string Key { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the value.</summary>
		[JsonPropertyName("value")]
		public string Value { get; set; }
	}
}
