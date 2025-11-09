using System.Runtime.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Enumeration to indicate the type of webhook event.
	/// </summary>
	public enum EventType
	{
		/// <summary>
		/// Default value.
		/// </summary>
		Unknown,

		/// <summary>
		/// A user has uninstalled or deauthorized your app.
		/// </summary>
		[EnumMember(Value = "app_deauthorized")]
		AppDeauthorized,

		/// <summary>
		/// A service issue has been encountered during a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.alert")]
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
		/// A meeting has ended.
		/// </summary>
		[EnumMember(Value = "meeting.ended")]
		MeetingEnded,

		/// <summary>
		/// A meeting live stream has started.
		/// </summary>
		[EnumMember(Value = "meeting.live_streaming_started")]
		MeetingLiveStreamStarted,

		/// <summary>
		/// A meeting live stream has stopped.
		/// </summary>
		[EnumMember(Value = "meeting.live_streaming_stopped")]
		MeetingLiveStreamStopped,

		/// <summary>
		/// A meeting host has admitted a participant from a waiting room to the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_admitted")]
		MeetingParticipantAdmitted,

		/// <summary>
		/// An attendee completed an end-of-meeting experience feedback survey for a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_feedback")]
		MeetingParticipantFeedback,

		/// <summary>
		/// An attendee has joined the meeting before the host.
		/// </summary>
		[EnumMember(Value = "meeting.participant_jbh_joined")]
		MeetingParticipantJoinedBeforeHost,

		/// <summary>
		/// An attendee is waiting for the host to join the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_jbh_waiting")]
		MeetingParticipantWaitingForHost,

		/// <summary>
		/// An attendee has joined a meting waiting room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_joined_waiting_room")]
		MeetingParticipantJoinedWaitingRoom,

		/// <summary>
		/// A meeting host has admitted a participant from a waiting room to the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_joined")]
		MeetingParticipantJoined,

		/// <summary>
		/// An attendee has left a meting waiting room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_left_waiting_room")]
		MeetingParticipantLeftWaitingRoom,

		/// <summary>
		/// A meeting participant has left the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_left")]
		MeetingParticipantLeft,

		/// <summary>
		/// A meeting participant who has already joined a meeting is sent back to the waiting room during the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_put_in_waiting_room")]
		MeetingParticipantSentToWaitingRoom,

		/// <summary>
		/// A host or meeting attendee changed their role during the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_role_changed")]
		MeetingParticipantRoleChanged,

		/// <summary>
		/// A meeting has been permanently deleted.
		/// </summary>
		[EnumMember(Value = "meeting.permanently_deleted")]
		MeetingPermanentlyDeleted,

		/// <summary>
		/// A meeting has been recovered.
		/// </summary>
		[EnumMember(Value = "meeting.recovered")]
		MeetingRecovered,

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
		/// A participant has registered for a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.registration_created")]
		MeetingRegistrationCreated,

		/// <summary>
		/// A meeting registration has been denied.
		/// </summary>
		[EnumMember(Value = "meeting.registration_denied")]
		MeetingRegistrationDenied,

		/// <summary>
		/// An attendee or the host has stoped sharing their screen during a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.sharing_ended")]
		MeetingSharingEnded,

		/// <summary>
		/// An attendee or the host has started sharing their screen during a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.sharing_started")]
		MeetingSharingStarted,

		/// <summary>
		/// A meeting has started.
		/// </summary>
		[EnumMember(Value = "meeting.started")]
		MeetingStarted,

		/// <summary>
		/// A meeting has been updated.
		/// </summary>
		[EnumMember(Value = "meeting.updated")]
		MeetingUpdated,

		/// <summary>
		/// A service issue has been encountered during a webinar.
		/// </summary>
		[EnumMember(Value = "webinar.alert")]
		WebinarServiceIssue,

		/// <summary>
		/// A webinar has been created.
		/// </summary>
		[EnumMember(Value = "webinar.created")]
		WebinarCreated,

		/// <summary>
		/// A webinar has been deleted.
		/// </summary>
		[EnumMember(Value = "webinar.deleted")]
		WebinarDeleted,

		/// <summary>
		/// A webinar has ended.
		/// </summary>
		[EnumMember(Value = "webinar.ended")]
		WebinarEnded,

		/// <summary>
		/// A webinar host or participant joined a webinar.
		/// </summary>
		[EnumMember(Value = "webinar.participant_joined")]
		WebinarParticipantJoined,

		/// <summary>
		/// A webinar host or participant left a webinar.
		/// </summary>
		[EnumMember(Value = "webinar.participant_left")]
		WebinarParticipantLeft,

		/// <summary>
		/// A webinar registration has been approved.
		/// </summary>
		[EnumMember(Value = "webinar.registration_approved")]
		WebinarRegistrationApproved,

		/// <summary>
		/// A webinar registration has been cancelled.
		/// </summary>
		[EnumMember(Value = "webinar.registration_cancelled")]
		WebinarRegistrationCancelled,

		/// <summary>
		/// A participant has registered for a webinar.
		/// </summary>
		[EnumMember(Value = "webinar.registration_created")]
		WebinarRegistrationCreated,

		/// <summary>
		/// A webinar registration has been denied.
		/// </summary>
		[EnumMember(Value = "webinar.registration_denied")]
		WebinarRegistrationDenied,

		/// <summary>
		/// An app user or account user has stopped sharing their screen during a webinar.
		/// </summary>
		[EnumMember(Value = "webinar.sharing_ended")]
		WebinarSharingEnded,

		/// <summary>
		/// An app user or account user has started sharing their screen during a webinar.
		/// </summary>
		[EnumMember(Value = "webinar.sharing_started")]
		WebinarSharingStarted,

		/// <summary>
		/// A webinar has started.
		/// </summary>
		[EnumMember(Value = "webinar.started")]
		WebinarStarted,

		/// <summary>
		/// A webinar has been updated.
		/// </summary>
		[EnumMember(Value = "webinar.updated")]
		WebinarUpdated,

		/// <summary>
		/// Zoom is requesting that you validate the endpoint.
		/// </summary>
		[EnumMember(Value = "endpoint.url_validation")]
		EndpointUrlValidation,

		/// <summary>
		/// The recording of meeting or webinar is available to view or download.
		/// </summary>
		[EnumMember(Value = "recording.completed")]
		RecordingCompleted,

		/// <summary>
		/// Meeting participant has joined a meeting breakout room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_joined_breakout_room")]
		MeetingParticipantJoinedBreakoutRoom,

		/// <summary>
		/// Meeting participant has left a meeting breakout room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_left_breakout_room")]
		MeetingParticipantLeftBreakoutRoom,

		/// <summary>
		/// User in a breakout room has begun content sharing.
		/// </summary>
		[EnumMember(Value = "meeting.breakout_room_sharing_started")]
		MeetingBreakoutRoomSharingStarted,

		/// <summary>
		/// User in a breakout room has ended content sharing.
		/// </summary>
		[EnumMember(Value = "meeting.breakout_room_sharing_ended")]
		MeetingBreakoutRoomSharingEnded,

		/// <summary>
		/// The summary of the meeting is made available for one of app users or account users.
		/// </summary>
		[EnumMember(Value = "meeting.summary_completed")]
		MeetingSummaryCompleted,

		/// <summary>
		/// App user or account user has permanently deleted a meeting summary.
		/// </summary>
		[EnumMember(Value = "meeting.summary_deleted")]
		MeetingSummaryDeleted,

		/// <summary>
		/// App user or account user has recovered a summary from the trash.
		/// </summary>
		[EnumMember(Value = "meeting.summary_recovered")]
		MeetingSummaryRecovered,

		/// <summary>
		/// App user or account user has shared a meeting summary.
		/// </summary>
		[EnumMember(Value = "meeting.summary_shared")]
		MeetingSummaryShared,

		/// <summary>
		/// App user or account user has temporarily deleted a meeting summary.
		/// </summary>
		[EnumMember(Value = "meeting.summary_trashed")]
		MeetingSummaryTrashed,

		/// <summary>
		/// App user or account user has updated a meeting summary.
		/// </summary>
		[EnumMember(Value = "meeting.summary_updated")]
		MeetingSummaryUpdated,

		/// <summary>
		/// Invitation's recipient has accepted an invitation to join a meeting or video call.
		/// </summary>
		[EnumMember(Value = "meeting.invitation_accepted")]
		MeetingInvitationAccepted,

		/// <summary>
		/// A user has sent an invitation to join a meeting or video call to one or more recipients.
		/// </summary>
		[EnumMember(Value = "meeting.invitation_dispatched")]
		MeetingInvitationDispatched,

		/// <summary>
		/// Invitation's recipient has rejected an invitation to join a meeting or video call.
		/// </summary>
		[EnumMember(Value = "meeting.invitation_rejected")]
		MeetingInvitationRejected,

		/// <summary>
		/// An invitation to join a meeting or video call to one or more recipients has timed out.
		/// </summary>
		[EnumMember(Value = "meeting.invitation_timeout")]
		MeetingInvitationTimeout,

		/// <summary>
		/// A user has accepted an invitation to join a meeting through phone (call out).
		/// </summary>
		[EnumMember(Value = "meeting.participant_phone_callout_accepted")]
		MeetingParticipantPhoneCalloutAccepted,

		/// <summary>
		/// An invitation to join a meeting through phone (call out) has timed out.
		/// </summary>
		[EnumMember(Value = "meeting.participant_phone_callout_missed")]
		MeetingParticipantPhoneCalloutMissed,

		/// <summary>
		/// A user has rejected an invitation to join a meeting through phone (call out).
		/// </summary>
		[EnumMember(Value = "meeting.participant_phone_callout_rejected")]
		MeetingParticipantPhoneCalloutRejected,

		/// <summary>
		/// A user's phone has started ringing when user is invited to join a meeting through phone (call out).
		/// </summary>
		[EnumMember(Value = "meeting.participant_phone_callout_ringing")]
		MeetingParticipantPhoneCalloutRinging,

		/// <summary>
		/// A user has joined a meeting through phone (call out) from a Zoom room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_room_system_callout_accepted")]
		MeetingParticipantRoomSystemCalloutAccepted,

		/// <summary>
		/// An invitation to join a meeting through phone (call out) from a Zoom room has failed.
		/// </summary>
		[EnumMember(Value = "meeting.participant_room_system_callout_failed")]
		MeetingParticipantRoomSystemCalloutFailed,

		/// <summary>
		/// An invitation to join a meeting through phone (call out) from a Zoom room has timed out.
		/// </summary>
		[EnumMember(Value = "meeting.participant_room_system_callout_missed")]
		MeetingParticipantRoomSystemCalloutMissed,

		/// <summary>
		/// A user has rejected an invitation to join a meeting through phone (call out) from a Zoom room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_room_system_callout_rejected")]
		MeetingParticipantRoomSystemCalloutRejected,

		/// <summary>
		/// A user's phone has started ringing when user is invited to join a meeting through phone (call out) from a Zoom room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_room_system_callout_ringing")]
		MeetingParticipantRoomSystemCalloutRinging,

		/// <summary>
		/// AI companion meeting assets (transcripts and summaries) have been deleted during a live meeting.
		/// </summary>
		[EnumMember(Value = "meeting.ai_companion_assets_deleted")]
		MeetingAiCompanionAssetsDeleted,

		/// <summary>
		/// A user has started AI companion during a live meeting.
		/// </summary>
		[EnumMember(Value = "meeting.ai_companion_started")]
		MeetingAiCompanionStarted,

		/// <summary>
		/// A user has stopped AI companion during a live meeting.
		/// </summary>
		[EnumMember(Value = "meeting.ai_companion_stopped")]
		MeetingAiCompanionStopped,

		/// <summary>
		/// AI companion has completed the meeting transcript after a meeting or webinar has ended.
		/// </summary>
		[EnumMember(Value = "meeting.aic_transcript_completed")]
		MeetingAicTranscriptCompleted,

		/// <summary>
		/// A meeting's message file is downloaded.
		/// </summary>
		[EnumMember(Value = "meeting.chat_message_file_downloaded")]
		MeetingChatMessageFileDownloaded,

		/// <summary>
		/// A meeting's message file is available to view or download.
		/// </summary>
		[EnumMember(Value = "meeting.chat_message_file_sent")]
		MeetingChatMessageFileSent,

		/// <summary>
		/// A user has sent a public or private chat message during a meeting using the in-meeting Zoom chat feature.
		/// </summary>
		[EnumMember(Value = "meeting.chat_message_sent")]
		MeetingChatMessageSent,

		/// <summary>
		/// A user has converted a meeting into a webinar.
		/// </summary>
		[EnumMember(Value = "meeting.converted_to_webinar")]
		MeetingConvertedToWebinar,

		/// <summary>
		/// A user has completed a connected device test.
		/// </summary>
		[EnumMember(Value = "meeting.device_tested")]
		MeetingDeviceTested,

		/// <summary>
		/// A user has posted a Zoom meeting link to a social media account.
		/// </summary>
		[EnumMember(Value = "meeting.risk_alert")]
		MeetingRiskAlert,

		/// <summary>
		/// A phone user has joined a meeting and bound to an attendee in the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_bind")]
		MeetingParticipantBind,

		/// <summary>
		/// An attendee waiting for the host to join the meeting has left.
		/// </summary>
		[EnumMember(Value = "meeting.participant_jbh_waiting_left")]
		MeetingParticipantWaitingForHostLeft,

		/// <summary>
		/// A message file from a webinar has been downloaded.
		/// </summary>
		[EnumMember(Value = "webinar.chat_message_file_downloaded")]
		WebinarChatMessageFileDownloaded,

		/// <summary>
		/// A message file of a webinar is available to view or download.
		/// </summary>
		[EnumMember(Value = "webinar.chat_message_file_sent")]
		WebinarChatMessageFileSent,

		/// <summary>
		/// A user has sent a public or private chat message during a webinar using chat feature.
		/// </summary>
		[EnumMember(Value = "webinar.chat_message_sent")]
		WebinarChatMessageSent,

		/// <summary>
		/// A user has converted a webinar into a meeting.
		/// </summary>
		[EnumMember(Value = "webinar.converted_to_meeting")]
		WebinarConvertedToMeeting,

		/// <summary>
		/// A phone user has joined a webinar and is bound to an attendee in the webinar.
		/// </summary>
		[EnumMember(Value = "webinar.participant_bind")]
		WebinarParticipantBind,

		/// <summary>
		/// An attendee has completed end-of-webinar feedback survey.
		/// </summary>
		[EnumMember(Value = "webinar.participant_feedback")]
		WebinarParticipantFeedback,

		/// <summary>
		/// A host or webinar attendee has changed their role during the webinar.
		/// </summary>
		[EnumMember(Value = "webinar.participant_role_changed")]
		WebinarParticipantRoleChanged,

		/// <summary>
		/// A scheduled webinar is permanently deleted.
		/// </summary>
		[EnumMember(Value = "webinar.permanently_deleted")]
		WebinarPermanentlyDeleted,

		/// <summary>
		/// A user has recovered a previously deleted webinar.
		/// </summary>
		[EnumMember(Value = "webinar.recovered")]
		WebinarRecovered,
	}
}
