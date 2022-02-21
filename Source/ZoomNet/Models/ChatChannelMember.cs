using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Member in a channel.
	/// </summary>
	public class ChatChannelMember
	{
		/// <summary>
		/// Gets or sets the member id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets a valid email address.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		[JsonPropertyName("role")]
		public ChatChannelRole Role { get; set; }
	}
}
