namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a service issue is encountered during a meeting.
	/// The following quality metrics can trigger an alert:
	/// - Unstable audio.
	/// - Unstable video.
	/// - Poor screen share quality.
	/// - High CPU usage.
	/// - Call reconnection problems.
	/// </summary>
	public class MeetingServiceIssueEvent : MeetingEvent
	{
		/// <summary>
		/// Gets or sets the issues that occured during the meeting.
		/// </summary>
		public string Issues { get; set; }
	}
}
