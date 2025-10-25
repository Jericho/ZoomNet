using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Shared user information.
	/// </summary>
	public class SharedUser
	{
		/// <summary>
		/// Gets or sets the shared user email.
		/// </summary>
		[JsonPropertyName("user_email")]
		public string Email { get; set; }
	}
}
