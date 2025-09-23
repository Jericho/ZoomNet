namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the user's reason when the user status is Not Ready.
	/// </summary>
	public enum ContactCenterUserSubStatus
	{
		/// <summary>Break.</summary>
		Break = 20,

		/// <summary>Meal.</summary>
		Meal = 21,

		/// <summary>Training.</summary>
		Training = 22,

		/// <summary>Meeting.</summary>
		Meeting = 23,

		/// <summary>EndShift.</summary>
		EndShift = 24,

		/// <summary>Forced.</summary>
		Forced = 25,
	}
}
