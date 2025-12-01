namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is is triggered when a call warm transfer is completed.
	/// </summary>
	/// <remarks>
	/// Applicable only to Zoom desktop clients (version 6.1.0 or later) using Zoom native phone numbers.
	/// Not applicable to IP phones or BYOC numbers.
	/// </remarks>
	public class PhoneWarmTransferCompletedEvent : PhoneCallTransferEvent
	{
	}
}
