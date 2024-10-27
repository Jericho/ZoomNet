using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate how is a phone call recorded.
	/// </summary>
	public enum PhoneCallRecordingType
	{
		/// <summary>
		/// Record on demand.
		/// </summary>
		[EnumMember(Value = "OnDemand")]
		OnDemand,

		/// <summary>
		/// Record automatically.
		/// </summary>
		[EnumMember(Value = "Automatic")]
		Automatic
	}
}
