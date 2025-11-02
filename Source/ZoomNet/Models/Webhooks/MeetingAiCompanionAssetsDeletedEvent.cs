namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when all AI Companion meeting assets (such as transcripts and summaries) are deleted during a live meeting.
	/// </summary>
	public class MeetingAiCompanionAssetsDeletedEvent : MeetingAiCompanionEvent
	{
		/// <summary>
		/// Gets or sets AI companion assets that were deleted.
		/// </summary>
		public string[] DeletedAssets { get; set; }
	}
}
