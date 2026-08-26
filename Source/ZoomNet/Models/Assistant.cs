using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Assistant.</summary>
	public class Assistant
	{
		/// <summary>Gets or sets the assistant's email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>Gets or sets the assistant id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
