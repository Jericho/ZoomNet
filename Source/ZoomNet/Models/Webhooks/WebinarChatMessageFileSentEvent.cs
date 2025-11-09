namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a message file of a webinar is available to view or download.
	/// </summary>
	public class WebinarChatMessageFileSentEvent : ChatMessageFileSentEvent
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
