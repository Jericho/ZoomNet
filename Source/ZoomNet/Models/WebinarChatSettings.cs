using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Webinar chat settings.
	/// </summary>
	public class WebinarChatSettings
	{
		/// <summary>Gets or sets the value indicating what groups a given attendee can chat with.</summary>
		[JsonProperty(PropertyName = "allow_attendees_chat_with")]
		public AttendeeChatType AttendeeChatType { get; set; }

		/// <summary>Gets or sets a value indicating whether to automatically save chat messages to a local file on the host's computer when the webinar ends.</summary>
		[JsonProperty(PropertyName = "allow_auto_save_local_chat_file")]
		public bool AutomaticallySaveChat { get; set; }

		/// <summary>Gets or sets the value indicating what groups a given panelist can chat with.</summary>
		[JsonProperty(PropertyName = "allow_panelists_chat_with")]
		public WebinarChatType PanelistChatType { get; set; }

		/// <summary>Gets or sets a value indicating whether to allow panelists to send direct messages to other panelists.</summary>
		[JsonProperty(PropertyName = "allow_panelists_send_direct_message")]
		public bool AllowPanelistsToSendDirectMessages { get; set; }

		/// <summary>Gets or sets the value indicating whether to allow attendees to save webinar chats.</summary>
		[JsonProperty(PropertyName = "allow_users_save_chats")]
		public AttendeeChatSaveType AttendeeChatSaveType { get; set; }

		/// <summary>Gets or sets the default value indicating what groups a given attendee can chat with.</summary>
		[JsonProperty(PropertyName = "default_attendees_chat_with")]
		public WebinarChatType DefaultAttendeeChatSaveType { get; set; }

		/// <summary>Gets or sets a value indicating whether participants can send chat messages.</summary>
		[JsonProperty(PropertyName = "enable")]
		public bool Enabled { get; set; }
	}
}
