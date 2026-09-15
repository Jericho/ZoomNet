using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Enumeration to indicate the type of meeting resource.</summary>
	public enum MeetingResourceType
	{
		/// <summary>Whiteboard.</summary>
		[EnumMember(Value = "whiteboard")]
		Whiteboard,
	}
}
