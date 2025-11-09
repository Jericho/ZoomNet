namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user sends a public or private chat message during a webinar using the chat feature.
	/// </summary>
	public class WebinarChatMessageSentEvent : ChatMessageSentEvent
	{
		/// <summary>
		/// Gets or sets the webinar id.
		/// </summary>
		public long WebinarId { get; set; }

		/// <summary>
		/// Gets or sets the webinar instance uuid.
		/// </summary>
		public string WebinarUuid { get; set; }
	}
}
