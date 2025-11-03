namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a user starts the AI Companion during a live meeting.
	/// </summary>
	public class MeetingAiCompanionStartedEvent : MeetingAiCompanionEvent
	{
		/// <summary>
		/// Gets or sets a value indicating whether AI companion's meeting questions feature was started.
		/// </summary>
		public bool Questions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether AI companion's meeting summary feature was started.
		/// </summary>
		public bool Summary { get; set; }
	}
}
