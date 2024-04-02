namespace ZoomNet.Models
{
	/// <summary>
	/// SMS message types.
	/// </summary>
	public enum SmsType
	{
		/// <summary>
		/// SMS.
		/// </summary>
		Sms = 1,

		/// <summary>
		/// MMS.
		/// </summary>
		Mms = 2,

		/// <summary>
		/// Group SMS.
		/// </summary>
		GroupSms = 3,

		/// <summary>
		/// Group MMS.
		/// </summary>
		GroupMms = 4,

		/// <summary>
		/// International SMS.
		/// </summary>
		InternationalSms = 5,

		/// <summary>
		/// MSG_ON_NET.
		/// </summary>
		MsgOnNet = 6
	}
}
