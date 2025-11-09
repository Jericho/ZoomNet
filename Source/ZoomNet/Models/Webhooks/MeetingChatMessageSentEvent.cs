namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user sends a public or private chat message during a meeting using the in-meeting Zoom chat feature.
	/// </summary>
	public class MeetingChatMessageSentEvent : ChatMessageSentEvent
	{
		/// <summary>
		/// Gets or sets the meeting id.
		/// </summary>
		public long MeetingId { get; set; }

		/// <summary>
		/// Gets or sets the meeting instance uuid.
		/// </summary>
		public string MeetingUuid { get; set; }
	}
}
