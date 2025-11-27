namespace ZoomNet.Models
{
	/// <summary>
	/// The Opt-In status between the Zoom Phone user number and consumer phone number.
	/// </summary>
	public enum SmsOptInStatus
	{
		/// <summary>
		/// The Opt-In status for the Zoom Phone user number cannot be determined.
		/// This may occur when the Zoom Phone user number is not associated with a Campaign, among other reasons.
		/// </summary>
		Default = 0,

		/// <summary>
		/// This is a new SMS session and an Opt-In message must be sent first.
		/// </summary>
		NewSession = 1,

		/// <summary>
		/// An Opt-In message has been sent and a response is pending.
		/// </summary>
		PendingResponse = 2,

		/// <summary>
		/// An Opt-In message has been sent and the recipient has consented to receive additional SMS messages.
		/// </summary>
		Confirmed = 3,

		/// <summary>
		/// An Opt-In message has been sent but the recipient has declined to receive additional SMS messages,
		/// or the number has been configured to block all outbound SMS messages.
		/// </summary>
		Declined = 4,
	}
}
