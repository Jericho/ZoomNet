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
		/// A user has uninstalled or deauthorized your app.
		/// </summary>
		[EnumMember(Value = "app_deauthorized")]
		AppDeauthorized,

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

		/// <summary>
		/// A participant has registered for a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.registration_created")]
		MeetingRegistrationCreated,

		/// <summary>
		/// A meeting registration has been approved.
		/// </summary>
		[EnumMember(Value = "meeting.registration_approved")]
		MeetingRegistrationApproved,

		/// <summary>
		/// A meeting registration has been cancelled.
		/// </summary>
		[EnumMember(Value = "meeting.registration_cancelled")]
		MeetingRegistrationCancelled,

		/// <summary>
		/// A meeting registration has been denied.
		/// </summary>
		[EnumMember(Value = "meeting.registration_denied")]
		MeetingRegistrationDenied,

		/// <summary>
		/// An attendee or the host has started sharing their screen.
		/// </summary>
		[EnumMember(Value = "meeting.sharing_started")]
		MeetingSharingStarted,

		/// <summary>
		/// An attendee or the host has stoped sharing their screen.
		/// </summary>
		[EnumMember(Value = "meeting.sharing_ended")]
		MeetingSharingEnded,
	}
}
