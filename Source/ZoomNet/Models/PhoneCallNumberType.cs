namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the phone call number type (for callee or caller).
	/// </summary>
	public enum PhoneCallNumberType
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
