using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a folder in the Zoom Rooms Digital Signage content library.</summary>
	public class DigitalSignageContentFolder
	{
		/// <summary>Gets or sets the folder ID.</summary>
		[JsonPropertyName("folder_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the folder.</summary>
		[JsonPropertyName("folder_name")]
		public string Name { get; set; }
	}
}
