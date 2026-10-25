using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Assistant.</summary>
	public record Assistant
	{
		/// <summary>Gets or sets the assistant's email address.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the assistant id.</summary>
		[JsonPropertyName("id")]
		public string Id { get; init; }
	}
}
