using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the URL to join the meeting.
		/// </summary>
		/// <value>The join URL.</value>
		[JsonProperty(PropertyName = "join_url", NullValueHandling = NullValueHandling.Ignore)]
		public string JoinUrl { get; set; }
	}
}
