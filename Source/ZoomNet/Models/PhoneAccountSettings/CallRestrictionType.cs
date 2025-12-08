namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Call forwarding restriction type.
	/// </summary>
	public enum CallRestrictionType
	{
		/// <summary>
		/// Low restriction (external numbers are not allowed).
		/// </summary>
		Low = 1,

		/// <summary>
		/// Medium restriction (external numbers and external contacts are not allowed).
		/// </summary>
		Medium = 2,

		/// <summary>
		/// High restriction (external numbers, unrecorded external contacts, and internal extensions without inbound automatic recording are not allowed).
		/// </summary>
		High = 3,

		/// <summary>
		/// No restriction.
		/// </summary>
		None = 4,
	}
}
