using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Participant.
	/// </summary>
	public class Participant
	{
		/// <summary>
		/// Gets or sets the participant uuid.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[JsonPropertyName("id")]
		public string Uuid { get; set; }

		/// <summary>
		/// Gets or sets the participant's email address.
		/// </summary>
		[JsonPropertyName("user_email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the participant's display name.
		/// </summary>
		[JsonPropertyName("name")]
		public string DisplayName { get; set; }
	}
}
