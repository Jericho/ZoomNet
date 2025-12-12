namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// The action when a call is blocked.
	/// </summary>
	public enum BlockCallAction
	{
		/// <summary>
		/// Forward to voicemail/videomail.
		/// </summary>
		ForwardToVoicemail = 0,

		/// <summary>
		/// Disconnect the call.
		/// </summary>
		Disconnect = 9,
	}
}
