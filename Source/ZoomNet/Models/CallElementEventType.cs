using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// An event type within a call log.
	/// </summary>
	public enum CallElementEventType
	{
		/// <summary>Incoming call.</summary>
		[EnumMember(Value = "incoming")]
		Incoming,

		/// <summary>Call transfer from Zoom contact center.</summary>
		[EnumMember(Value = "transfer_from_zoom_contact_center")]
		TransferFromZoomContactCenter,

		/// <summary>Incoming call from shared line.</summary>
		[EnumMember(Value = "shared_line_incoming")]
		SharedLineIncoming,

		/// <summary>Outgoing call.</summary>
		[EnumMember(Value = "outgoing")]
		Outgoing,

		/// <summary>Call me on.</summary>
		[EnumMember(Value = "call_me_on")]
		CallMeOn,

		/// <summary>Outgoing call to Zoom contact center.</summary>
		[EnumMember(Value = "outgoing_to_zoom_contact_center")]
		OutgoingToZoomContactCenter,

		/// <summary>Warm call transfer.</summary>
		[EnumMember(Value = "warm_transfer")]
		WarmTransfer,

		/// <summary>Call forwarding.</summary>
		[EnumMember(Value = "forward")]
		Forward,

		/// <summary>Ring to member (of the call queue or shared line group).</summary>
		[EnumMember(Value = "ring_to_member")]
		RingToMember,

		/// <summary>Call overflow.</summary>
		[EnumMember(Value = "overflow")]
		Overflow,

		/// <summary>Direct call transfer.</summary>
		[EnumMember(Value = "direct_transfer")]
		DirectTransfer,

		/// <summary>Call barging (joining active call).</summary>
		[EnumMember(Value = "barge")]
		Barge,

		/// <summary>Call monitoring.</summary>
		[EnumMember(Value = "monitor")]
		Monitor,

		/// <summary>Call whispering (privately speaking to one person).</summary>
		[EnumMember(Value = "whisper")]
		Whisper,

		/// <summary>Call listening (privately).</summary>
		[EnumMember(Value = "listen")]
		Listen,

		/// <summary>Call takeover (replacing participant).</summary>
		[EnumMember(Value = "takeover")]
		Takeover,

		/// <summary>Conference call barging (joining active conference call).</summary>
		[EnumMember(Value = "conference_barge")]
		ConferenceBarge,

		/// <summary>Call parking.</summary>
		[EnumMember(Value = "park")]
		Park,

		/// <summary>Call timeout.</summary>
		[EnumMember(Value = "timeout")]
		Timeout,

		/// <summary>Picking up parked call.</summary>
		[EnumMember(Value = "park_pick_up")]
		ParkPickUp,

		/// <summary>Call merging.</summary>
		[EnumMember(Value = "merge")]
		Merge,

		/// <summary>Call sharing.</summary>
		[EnumMember(Value = "shared")]
		Shared,
	}
}
