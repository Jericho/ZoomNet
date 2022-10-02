using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A tracking field.
	/// </summary>
	public class TrackingField
	{
		/// <summary>
		/// Gets or sets the tracking field id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the tracking field.
		/// </summary>
		/// <value>
		/// The name of the tracking field.
		/// </value>
		[JsonPropertyName("field")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the array of recommended values.
		/// </summary>
		/// <value>
		/// The recommended values.
		/// </value>
		[JsonPropertyName("recommended_values")]
		public string[] RecommendedValues { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating whether the field is required.
		/// </summary>
		[JsonPropertyName("required")]
		public bool IsRequired { get; set; }

		/// <summary>
		/// Gets or sets a flag indicating whether the field is visible.
		/// </summary>
		[JsonPropertyName("visible")]
		public bool IsVisible { get; set; }
	}
}
