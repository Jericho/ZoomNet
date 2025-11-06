namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time someone posts a Zoom meeting link to a social media account.
	/// </summary>
	public class MeetingRiskAlertEvent : MeetingInfoEvent
	{
		/// <summary>
		/// Gets or sets the information about at-risk meeting notifier.
		/// </summary>
		public MeetingAtRiskDetails ArmnDetails { get; set; }
	}
}
