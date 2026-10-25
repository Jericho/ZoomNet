namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when a message file from a webinar is downloaded.</summary>
	public record WebinarChatMessageFileDownloadedEvent : ChatMessageFileDownloadedEvent
	{
		/// <summary>Gets or sets the webinar id.</summary>
		public long WebinarId { get; init; }

		/// <summary>Gets or sets the webinar instance uuid.</summary>
		public string WebinarUuid { get; init; }
	}
}
