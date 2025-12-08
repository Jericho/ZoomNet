namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// The deletion policy (soft/permanent).
	/// </summary>
	public enum DeleteDataPolicy
	{
		/// <summary>
		/// Soft delete.
		/// </summary>
		Soft = 1,

		/// <summary>
		/// Permanent delete.
		/// </summary>
		Permanent = 2,
	}
}
