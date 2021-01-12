using Newtonsoft.Json;

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
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets a valid email address.
		/// </summary>
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[JsonProperty(PropertyName = "first_name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[JsonProperty(PropertyName = "last_name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		[JsonProperty(PropertyName = "role")]
		public ChatChannelRole Role { get; set; }
	}
}
