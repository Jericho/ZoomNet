using System.Runtime.Serialization;
using ZoomNet.Utilities;

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
		[MultipleValuesEnumMember(DefaultValue = "On-demand", OtherValues = ["On-demand"])]
		OnDemand,

		/// <summary>
		/// Record automatically.
		/// </summary>
		[EnumMember(Value = "Automatic")]
		Automatic
	}
}
