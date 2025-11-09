namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a meeting's message file is downloaded.
	/// </summary>
	public class MeetingChatMessageFileDownloadedEvent : ChatMessageFileDownloadedEvent
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
