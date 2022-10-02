using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of a webinar template.
	/// </summary>
	public class WebinarTemplate
	{
		/// <summary>
		/// Gets or sets the Id of the template.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the Name of the template.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
