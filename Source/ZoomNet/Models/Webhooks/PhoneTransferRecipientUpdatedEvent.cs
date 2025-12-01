namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when the recipient of warm/blind transfer call is updated, to inform that the call connector has changed.
	/// </summary>
	/// <remarks>
	/// Applicable only to Zoom desktop clients (version 6.1.0 or later) using Zoom native phone numbers.
	/// Not applicable to IP phones or BYOC numbers.
	/// </remarks>
	public class PhoneTransferRecipientUpdatedEvent : PhoneCallTransferEvent
	{
		/// <summary>
		/// Gets or sets the recipient who received the warm/blind transfer call from the event owner.
		/// </summary>
		public WebhookCallTransferRecipient Recipient { get; set; }
	}
}
