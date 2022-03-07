using System.Text.Json.Serialization;

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
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the template.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the template.
		/// </summary>
		[JsonPropertyName("type")]
		public MeetingTemplateType Type { get; set; }
	}
}
