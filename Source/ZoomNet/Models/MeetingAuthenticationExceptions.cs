using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents meeting authentication exceptions.</summary>
	public class MeetingAuthenticationExceptions
	{
		/// <summary>Gets or sets the email address of the participant who will receive unique meeting invite links and bypass authentication.</summary>
		[JsonPropertyName("email")]
		public string EmailAddress { get; set; }

		/// <summary>Gets or sets the URL for participants to join the meeting.</summary>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }

		/// <summary>Gets or sets the participant's name.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
