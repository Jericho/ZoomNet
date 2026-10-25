namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a user sends a public or private chat message during a meeting using the in-meeting Zoom chat feature.</summary>
	public record MeetingChatMessageSentEvent : ChatMessageSentEvent
	{
		/// <summary>Gets or sets the meeting id.</summary>
		public long MeetingId { get; init; }

		/// <summary>Gets or sets the meeting instance uuid.</summary>
		public string MeetingUuid { get; init; }
	}
}
