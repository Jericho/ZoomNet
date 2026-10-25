using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents a folder in the Zoom Rooms Digital Signage content library.</summary>
	public record DigitalSignageContentFolder
	{
		/// <summary>Gets or sets the folder ID.</summary>
		[JsonPropertyName("folder_id")]
		public string Id { get; init; }

		/// <summary>Gets or sets the name of the folder.</summary>
		[JsonPropertyName("folder_name")]
		public string Name { get; init; }
	}
}
