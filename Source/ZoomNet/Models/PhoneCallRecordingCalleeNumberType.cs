namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the phone callee's number type.
	/// </summary>
	public enum PhoneCallRecordingCalleeNumberType
	{
		/// <summary>
		/// Internal number.
		/// </summary>
		Internal = 1,

		/// <summary>
		/// External number.
		/// </summary>
		External = 2,

		/// <summary>
		/// Customized emergency number.
		/// </summary>
		CustomizedEmergency = 3
	}
}
