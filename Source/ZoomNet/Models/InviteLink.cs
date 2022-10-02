using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Meeting invite link.
	/// </summary>
	public class InviteLink
	{
		/// <summary>
		/// Gets or sets the display name of the invited attendee.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the meeting.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonPropertyName("join_url")]
		public string JoinUrl { get; set; }
	}
}
