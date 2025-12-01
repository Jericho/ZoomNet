namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a call warm transfer is initiated.
	/// </summary>
	/// <remarks>
	/// Applicable only to Zoom desktop clients (version 6.1.0 or later) using Zoom native phone numbers.
	/// Not applicable to IP phones or BYOC numbers.
	/// </remarks>
	public class PhoneWarmTransferInitiatedEvent : PhoneCallTransferEvent
	{
		/// <summary>
		/// Gets or sets the newly generated call ID when the call is transferred.
		/// </summary>
		public string TransferCallId { get; set; }
	}
}
