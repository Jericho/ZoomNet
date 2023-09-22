namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of disclaimer for phone call recording.
	/// </summary>
	public enum PhoneCallRecordingDisclaimerStatus
	{
		/// <summary>
		/// Passive/implicit.
		/// </summary>
		Implicit = 0,

		/// <summary>
		/// Agree (active/explicit and press 1).
		/// </summary>
		Agree = 1,

		/// <summary>
		/// Passive agree (active/explicit and no press).
		/// </summary>
		PassiveAgree = 2
	}
}
