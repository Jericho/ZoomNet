using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Chat channel settings.
	/// </summary>
	public class ChatChannelSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether members can see files previously posted to the channel.
		/// </summary>
		[JsonProperty(PropertyName = "new_members_can_see_previous_messages_files")]
		public bool CanSeePreviousMessageFiles { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether external users can be added to the channel.
		/// </summary>
		[JsonProperty(PropertyName = "allow_to_add_external_users")]
		public bool CanAddExternalUsers { get; set; }

		/// <summary>
		/// Gets or sets the permissions.
		/// </summary>
		[JsonProperty(PropertyName = "posting_permissions")]
		public int Permissions { get; set; }
	}
}
