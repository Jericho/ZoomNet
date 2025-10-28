namespace ZoomNet.Models
{
	/// <summary>
	/// Failure reasons for joining a meeting through phone (call out) from a Zoom room.
	/// </summary>
	public enum MeetingRoomCalloutFailureReason
	{
		/// <summary>
		/// Other failure reason.
		/// </summary>
		Other = 0,

		/// <summary>
		/// Encryption failure.
		/// </summary>
		EncryptionFail = 1,

		/// <summary>
		/// Disconnected by remote party.
		/// </summary>
		DisconnectedByRemote = 2,

		/// <summary>
		/// Retry error.
		/// </summary>
		Retry = 3,

		/// <summary>
		/// Bad sequence error.
		/// </summary>
		BadSeq = 4,

		/// <summary>
		/// Call limit error.
		/// </summary>
		CallLimit = 5,

		/// <summary>
		/// Not registered error.
		/// </summary>
		NotRegistered = 6,

		/// <summary>
		/// Timeout error.
		/// </summary>
		Timeout = 7,

		/// <summary>
		/// Bad address error.
		/// </summary>
		BadAddress = 8,

		/// <summary>
		/// Unreachable error.
		/// </summary>
		Unreachable = 9,

		/// <summary>
		/// Disconnected by local party.
		/// </summary>
		DisconnectByLocal = 10,

		/// <summary>
		/// Server internal error.
		/// </summary>
		ServerInternalError = 11,

		/// <summary>
		/// Free port exceeded error.
		/// </summary>
		ExceedFreePort = 12,

		/// <summary>
		/// Connection error.
		/// </summary>
		ConnectError = 13,

		/// <summary>
		/// Proxy connection error.
		/// </summary>
		ProxyConnectError = 14,
	}
}
