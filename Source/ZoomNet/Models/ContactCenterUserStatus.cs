namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the status of a contact center user.
	/// </summary>
	public enum ContactCenterUserStatus
	{
		/// <summary>Offline.</summary>
		Offline = 0,

		/// <summary>Ready.</summary>
		Ready = 1,

		/// <summary>Not Ready.</summary>
		NotReady = 2,

		/// <summary>Occupied.</summary>
		Occupied = 3,

		/// <summary>Custom.</summary>
		Custom = 4,
	}
}
