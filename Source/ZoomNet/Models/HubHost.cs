using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A Host.</summary>
	public record HubHost
	{
		/// <summary>Gets or sets the email address of the hub host.</summary>
		[JsonPropertyName("email")]
		public string Email { get; init; }

		/// <summary>Gets or sets the user ID of the hub host.</summary>
		[JsonPropertyName("host_user_id")]
		public string Id { get; init; }
	}
}
