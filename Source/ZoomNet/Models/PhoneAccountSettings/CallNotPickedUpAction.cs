namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// The action when a parked call is not picked up.
	/// </summary>
	public enum CallNotPickedUpAction
	{
		/// <summary>
		/// Forward to voicemail of the parker.
		/// </summary>
		ForwardToVoicemail = 0,

		/// <summary>
		/// Disconnect the call.
		/// </summary>
		Disconnect = 9,

		/// <summary>
		/// Forward call to another extension.
		/// </summary>
		ForwardToAnotherExtension = 50,

		/// <summary>
		/// Ring back to parker.
		/// </summary>
		RingBackToParker = 100,
	}
}
