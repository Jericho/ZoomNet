namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// The actions taken when SMS etiquette policy is triggered.
	/// </summary>
	public enum SmsEtiquettePolicyAction
	{
		/// <summary>
		/// Ask user to confirm message sending.
		/// </summary>
		AskUserToConfirm = 1,

		/// <summary>
		/// Block the message.
		/// </summary>
		BlockMessage = 2,
	}
}
