using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Connect type of the call used in call element.
	/// </summary>
	public enum CallElementConnectType
	{
		/// <summary>Internal call.</summary>
		[EnumMember(Value = "internal")]
		Internal,

		/// <summary>External call.</summary>
		[EnumMember(Value = "external")]
		External,
	}
}
