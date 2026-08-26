using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Phone call user information.</summary>
	public class PhoneCallUser
	{
		/// <summary>Gets or sets the user extension number.</summary>
		[JsonPropertyName("extension_number")]
		public string ExtensionNumber { get; set; }

		/// <summary>Gets or sets the user name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
