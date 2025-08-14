using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of digital signage resource.
	/// </summary>
	public enum SignageResourceType
	{
		/// <summary>Content files.</summary>
		[EnumMember(Value = "content")]
		Content,

		/// <summary>Folder where the content files are located.</summary>
		[EnumMember(Value = "folder")]
		Folder
	}
}
