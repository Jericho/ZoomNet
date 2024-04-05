using System.Text.Json.Serialization;

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
		[JsonPropertyName("new_members_can_see_previous_messages_files")]
		public bool NewMembersCanSeePreviousMessageFiles { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether external users can be added to the channel.
		/// </summary>
		[JsonPropertyName("allow_to_add_external_users")]
		public bool CanAddExternalUsers { get; set; }

		/// <summary>
		/// Gets or sets the value indicating who can post to a channel.
		/// </summary>
		[JsonPropertyName("posting_permissions")]
		public ChatChannelPostingPermissions PostingPermissions { get; set; }

		/// <summary>
		/// Gets or sets the value indicating who can add new channel members.
		/// </summary>
		[JsonPropertyName("add_member_permissions")]
		public ChatChannelAddMemberPermissions AddMemberPermissions { get; set; }

		/// <summary>
		/// Gets or sets the value indicating who can use @all.
		/// </summary>
		[JsonPropertyName("mention_all_permissions")]
		public ChatChannelMentionAllPermissions MentionAllPermissions { get; set; }
	}
}
