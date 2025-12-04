using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of synchronization.
	/// </summary>
	public enum SynchronizationType
	{
		/// <summary>Full synchronization.</summary>
		[EnumMember(Value = "FSync")]
		Full,

		/// <summary>Increase synchronization.</summary>
		[EnumMember(Value = "ISync")]
		Increase,

		/// <summary>Backward synchronization.</summary>
		[EnumMember(Value = "BSync")]
		Backward,
	}
}
