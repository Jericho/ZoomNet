namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the incoming call element records are made available for the callee to view.
	/// </summary>
	public class PhoneCalleeCallElementCompletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets completed call elements information.
		/// </summary>
		public CallElement[] CallElements { get; set; }
	}
}
