using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents a member of a call queue, which can be a user or a common area.
	/// </summary>
	public class CallQueueMember
	{
		/// <summary>
		/// Gets or sets the member id.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the level of the member.
		/// </summary>
		[JsonPropertyName("level")]
		public string Level { get; set; }

		/// <summary>
		/// Gets or sets the name of the user or common area.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user can receive calls. It displays if the level is user.
		/// </summary>
		[JsonPropertyName("receive_call")]
		public bool ReceiveCall { get; set; }

		/// <summary>
		/// Gets or sets the extension ID of the user or common area.
		/// </summary>
		[JsonPropertyName("extension_id")]
		public string ExtensionId { get; set; }
	}
}
