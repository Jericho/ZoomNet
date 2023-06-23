using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the file extension type of the recording file.
	/// </summary>
	public enum RecordingFileExtension
	{
		/// <summary>Not specified.</summary>
		[EnumMember(Value = "")]
		NotSpecified,

		/// <summary>MP4.</summary>
		[EnumMember(Value = "MP4")]
		MP4,

		/// <summary>M4A.</summary>
		[EnumMember(Value = "M4A")]
		M4A,

		/// <summary>TXT.</summary>
		[EnumMember(Value = "TXT")]
		TXT,

		/// <summary>TVTT.</summary>
		[EnumMember(Value = "VTT")]
		VTT,

		/// <summary>CSV.</summary>
		[EnumMember(Value = "CSV")]
		CSV,

		/// <summary>JSON.</summary>
		[EnumMember(Value = "JSON")]
		JSON,

		/// <summary>JPG.</summary>
		[EnumMember(Value = "JPG")]
		JPG,
	}
}
