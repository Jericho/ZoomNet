namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the outgoing call element record is made available for the caller to view.
	/// </summary>
	public class PhoneCallerCallElementCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets completed call elements information.
		/// </summary>
		public CallElement[] CallElements { get; set; }
	}
}
