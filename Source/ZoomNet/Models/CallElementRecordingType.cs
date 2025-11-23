using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Recording type used in call element.
	/// </summary>
	public enum CallElementRecordingType
	{
		/// <summary>Ad-hoc recording.</summary>
		[EnumMember(Value = "ad-hoc")]
		AdHoc,

		/// <summary>Automatic recording.</summary>
		[EnumMember(Value = "automatic")]
		Automatic,
	}
}
