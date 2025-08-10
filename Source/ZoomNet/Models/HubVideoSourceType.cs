using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the video source type.
	/// </summary>
	public enum HubVideoSourceType
	{
		/// <summary>Unknown (or not specified).</summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>Recording file.</summary>
		[EnumMember(Value = "RECORDING ")]
		Recording,

		/// <summary>Manually uploaded file.</summary>
		[EnumMember(Value = "UPLOAD")]
		Upload,

		/// <summary>This is a folder for organizing videos.</summary>
		[EnumMember(Value = "FOLDER")]
		Folder,
	}
}
