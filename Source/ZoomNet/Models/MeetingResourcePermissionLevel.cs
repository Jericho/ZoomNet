using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the permission level for users to access the meeting resource.</summary>
	public enum MeetingResourcePermissionLevel
	{
		/// <summary>Editor.</summary>
		[EnumMember(Value = "editor")]
		Editor,

		/// <summary>Commenter.</summary>
		[EnumMember(Value = "commenter")]
		Commenter,

		/// <summary>Viewer.</summary>
		[EnumMember(Value = "viewer")]
		Viewer,
	}
}
