namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Indicates how parked calls are assigned to a BLF (Busy Lamp Field) key.
	/// </summary>
	public enum ParkedCallsAssignmentSequence
	{
		/// <summary>
		/// The call is parked at a randomly selected BLF key.
		/// </summary>
		Random = 0,

		/// <summary>
		/// The call is parked at the next available BLF key.
		/// </summary>
		Sequential = 1,
	}
}
