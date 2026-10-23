using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Represents the mode of the waiting room.</summary>
	public enum MeetingWaitingRoomMode
	{
		/// <summary>Use the web portal setting.</summary>
		[EnumMember(Value = "follow_setting")]
		UseWebPortalSetting,

		/// <summary>Custom.</summary>
		[EnumMember(Value = "custom")]
		Custom,
	}
}
