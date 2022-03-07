using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate who the payee is.
	/// </summary>
	public enum PayMode
	{
		/// <summary>
		/// Master account holder pays.
		/// </summary>
		[EnumMember(Value = "master")]
		Master,

		/// <summary>
		/// Sub account holder pays.
		/// </summary>
		[EnumMember(Value = "sub")]
		SubAccount
	}
}
