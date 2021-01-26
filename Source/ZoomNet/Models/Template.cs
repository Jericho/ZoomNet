using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of a webinar template.
	/// </summary>
	public class Template
	{
		/// <summary>
		/// Gets or sets the Id of the template.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the Name of the template.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
	}
}
