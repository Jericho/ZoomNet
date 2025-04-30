using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of workspace.
	/// </summary>
	public enum WorkspaceType
	{
		/// <summary>Desk.</summary>
		[EnumMember(Value = "desk")]
		Desk,

		/// <summary>Room.</summary>
		[EnumMember(Value = "room")]
		Room
	}
}
