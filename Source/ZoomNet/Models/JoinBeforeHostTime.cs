namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the time limits within which a participant can join a meeting before the meeting's host.
	/// </summary>
	public enum JoinBeforeHostTime
	{
		/// <summary>
		/// Participants are allowed to join the meeting at anytime.
		/// </summary>
		Anytime = 0,

		/// <summary>
		/// Participants are allowed to join the meeting 5 minutes before the meeting's start time.
		/// </summary>
		FiveMinutes = 5,

		/// <summary>
		/// Participants are allowed to join the meeting 10 minutes before the meeting's start time.
		/// </summary>
		TenMinutes = 10,
	}
}
