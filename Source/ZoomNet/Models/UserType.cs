namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of a user.
	/// </summary>
	public enum UserType
	{
		/// <summary>Basic.</summary>
		Basic = 1,

		/// <summary>Licensed.</summary>
		Licensed = 2,

		/// <summary>On-premise.</summary>
		OnPremise = 3,

		/// <summary>None.</summary>
		/// <remarks>This can only be set with <see cref="UserCreateType.SSo"/>.</remarks>
		None = 99,
	}
}
