using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Host.
	/// </summary>
	public class HubHost
	{
		/// <summary>Gets or sets the user ID of the hub host.</summary>
		[JsonPropertyName("host_user_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the email address of the hub host.</summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }
	}
}
