namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// The action to take when the user is busy on another call.
	/// </summary>
	public enum BusyOnAnotherCallActionType
	{
		/// <summary>
		/// Forward to a Voicemail.
		/// </summary>
		ForwardToVoicemail = 1,

		/// <summary>
		/// Forward to the User.
		/// </summary>
		ForwardToUser = 2,

		/// <summary>
		/// Forward to the Common Area.
		/// </summary>
		ForwardToCommonArea = 4,

		/// <summary>
		/// Forward to the Auto Receptionist.
		/// </summary>
		ForwardToAutoReceptionist = 6,

		/// <summary>
		/// Forward to a Call Queue.
		/// </summary>
		ForwardToCallQueue = 7,

		/// <summary>
		/// Forward to a Shared Line Group.
		/// </summary>
		ForwardToSharedLineGroup = 8,

		/// <summary>
		/// Forward to an External Contact.
		/// </summary>
		ForwardToExternalContact = 9,

		/// <summary>
		/// Forward to an external Phone Number.
		/// </summary>
		ForwardToPhoneNumber = 10,

		/// <summary>
		/// Play a message, then disconnect.
		/// </summary>
		PlayMessageThenDisconnect = 12,

		/// <summary>
		/// Call waiting.
		/// </summary>
		CallWaiting = 21,

		/// <summary>
		/// Play a busy signal.
		/// </summary>
		PlayBusySignal = 22
	}
}
