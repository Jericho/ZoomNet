using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Enumeration to indicate the type of webhook event.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EventType
	{
		/// <summary>
		/// A service issue has been encountered during a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.alerts")]
		MeetingServiceIssue,

		/// <summary>
		/// A meeting has been created.
		/// </summary>
		[EnumMember(Value = "meeting.created")]
		MeetingCreated,

		/// <summary>
		/// A meeting has been deleted.
		/// </summary>
		[EnumMember(Value = "meeting.deleted")]
		MeetingDeleted,

		/// <summary>
		/// A meeting has been updated.
		/// </summary>
		[EnumMember(Value = "meeting.updated")]
		MeetingUpdated,

		/// <summary>
		/// A meeting has been permanently deleted.
		/// </summary>
		[EnumMember(Value = "meeting.permanently_deleted")]
		MeetingPermanentlyDeleted,

		/// <summary>
		/// A meeting has started.
		/// </summary>
		[EnumMember(Value = "meeting.started")]
		MeetingStarted,

		/// <summary>
		/// A meeting has ended.
		/// </summary>
		[EnumMember(Value = "meeting.ended")]
		MeetingEnded,

		/// <summary>
		/// A meeting has been recovered.
		/// </summary>
		[EnumMember(Value = "meeting.recovered")]
		MeetingRecovered,
	}
}
