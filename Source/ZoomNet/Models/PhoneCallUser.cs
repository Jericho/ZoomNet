using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Phone call user information.
	/// </summary>
	public class PhoneCallUser
	{
		/// <summary>Gets or sets the user name.</summary>
		/// <value>The user name.</value>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the user extension number.</summary>
		/// <value>The user extension number.</value>
		[JsonPropertyName("extension_number")]
		public string ExtensionNumber { get; set; }
	}
}
