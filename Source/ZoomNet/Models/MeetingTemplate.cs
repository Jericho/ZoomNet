using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Details of a meeting template.
	/// </summary>
	public class MeetingTemplate
	{
		/// <summary>
		/// Gets or sets the unique identifier of the template.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the template.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the template.
		/// </summary>
		[JsonProperty(PropertyName = "type")]
		public MeetingTemplateType Type { get; set; }
	}
}
