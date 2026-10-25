using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Shared user information.</summary>
	public record SharedUser
	{
		/// <summary>Gets or sets the shared user email.</summary>
		[JsonPropertyName("user_email")]
		public string Email { get; init; }
	}
}
