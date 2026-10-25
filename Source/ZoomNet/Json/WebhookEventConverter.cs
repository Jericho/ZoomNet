using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using static ZoomNet.Internal;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an <see cref="Models.Webhooks.Event"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class WebHookEventConverter : ZoomNetJsonConverter<Models.Webhooks.Event>
	{
		private readonly bool _throwWhenUnknownEventType;

		public WebHookEventConverter(bool throwWhenUnknownEventType = true)
		{
			_throwWhenUnknownEventType = throwWhenUnknownEventType;
		}

		public override Models.Webhooks.Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var timestamp = rootElement.GetPropertyValue<long>("event_ts").FromUnixTime(UnixTimePrecision.Milliseconds);
			var eventTypeName = rootElement.GetPropertyValue("event", string.Empty);
			var eventType = Models.Webhooks.EventType.Unknown;

			if (!eventTypeName.TryToEnum(out eventType))
			{
				if (_throwWhenUnknownEventType)
				{
					throw new ArgumentException($"{eventTypeName} is not a recognized event type.", nameof(eventTypeName));
				}

				eventType = Models.Webhooks.EventType.Unknown;
			}

			var webHookEvent = ParsePayloadToWebhookEvent(rootElement, eventType, options) with
			{
				EventType = eventType,
				Timestamp = timestamp
			};

			return webHookEvent;
		}

		private static Models.Webhooks.Event ParsePayloadToWebhookEvent(JsonElement rootElement, Models.Webhooks.EventType eventType, JsonSerializerOptions options)
		{
			var payloadJsonProperty = rootElement.GetProperty("payload", true).Value;

			switch (eventType)
			{
				case Models.Webhooks.EventType.Unknown:
					return new UnknownEvent
					{
						EventTypeName = rootElement.GetPropertyValue("event", string.Empty),
						Payload = payloadJsonProperty
					};
				case Models.Webhooks.EventType.AppDeauthorized:
					return payloadJsonProperty.ToObject<AppDeauthorizedEvent>(options);
				case Models.Webhooks.EventType.MeetingServiceIssue:
					return payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(options) with
					{
						Issues = payloadJsonProperty.GetPropertyValue("object/issues", Array.Empty<string>())
					};
				case Models.Webhooks.EventType.MeetingCreated:
					return payloadJsonProperty.ToObject<MeetingCreatedEvent>(options);
				case Models.Webhooks.EventType.MeetingDeleted:
					return payloadJsonProperty.ToObject<MeetingDeletedEvent>(options);
				case Models.Webhooks.EventType.MeetingUpdated:
					var oldMeetingValues = payloadJsonProperty.GetProperty("old_object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					var newMeetingValues = payloadJsonProperty.GetProperty("object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					return payloadJsonProperty.ToObject<MeetingUpdatedEvent>(options) with
					{
						UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty),
						ModifiedFields = oldMeetingValues.Keys
							.Where(key => !oldMeetingValues[key].Equals(newMeetingValues[key]))
							.Select(key => (FieldName: key, OldValue: oldMeetingValues[key], NewValue: newMeetingValues[key]))
							.ToArray(),
						MeetingFields = oldMeetingValues.Keys
							.Where(key => oldMeetingValues[key].Equals(newMeetingValues[key]))
							.Select(key => (FieldName: key, Value: oldMeetingValues[key]))
							.ToArray()
					};
				case Models.Webhooks.EventType.MeetingPermanentlyDeleted:
					return payloadJsonProperty.ToObject<MeetingPermanentlyDeletedEvent>(options);
				case Models.Webhooks.EventType.MeetingStarted:
					return payloadJsonProperty.ToObject<MeetingStartedEvent>(options);
				case Models.Webhooks.EventType.MeetingEnded:
					return payloadJsonProperty.ToObject<MeetingEndedEvent>(options) with
					{
						EndTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/end_time")
					};
				case Models.Webhooks.EventType.MeetingRecovered:
					return payloadJsonProperty.ToObject<MeetingRecoveredEvent>(options);
				case Models.Webhooks.EventType.MeetingRegistrationCreated:
					return payloadJsonProperty.ToObject<MeetingRegistrationCreatedEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingRegistrationApproved:
					return payloadJsonProperty.ToObject<MeetingRegistrationApprovedEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingRegistrationCancelled:
					return payloadJsonProperty.ToObject<MeetingRegistrationCancelledEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingRegistrationDenied:
					return payloadJsonProperty.ToObject<MeetingRegistrationDeniedEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingSharingStarted:
					return payloadJsonProperty.ToObject<MeetingSharingStartedEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty),
						ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingSharingEnded:
					return payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty),
						ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHost:
					return payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHostLeft:
					return payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostLeftEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantJoinedBeforeHost:
					return payloadJsonProperty.ToObject<MeetingParticipantJoinedBeforeHostEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantJoinedWaitingRoom:
					return payloadJsonProperty.ToObject<MeetingParticipantJoinedWaitingRoomEvent>(options) with
					{
						JoinedOn = ParseParticipantDateTime(payloadJsonProperty),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantLeftWaitingRoom:
					return payloadJsonProperty.ToObject<MeetingParticipantLeftWaitingRoomEvent>(options) with
					{
						LeftOn = ParseParticipantDateTime(payloadJsonProperty),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantAdmitted:
					return payloadJsonProperty.ToObject<MeetingParticipantAdmittedEvent>(options) with
					{
						AdmittedOn = ParseParticipantDateTime(payloadJsonProperty),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantBind:
					return payloadJsonProperty.ToObject<MeetingParticipantBindEvent>(options) with
					{
						BindUserId = payloadJsonProperty.GetPropertyValue<string>("object/participant/bind_user_id"),
						BindParticipantUuid = payloadJsonProperty.GetPropertyValue("object/participant/bind_participant_uuid", string.Empty),
						BoundOn = ParseParticipantDateTime(payloadJsonProperty),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantFeedback:
					return payloadJsonProperty.ToObject<MeetingParticipantFeedbackEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty),
						Feedback = payloadJsonProperty.GetPropertyValue<MeetingParticipantFeedback>("object/participant/feedback")
					};
				case Models.Webhooks.EventType.MeetingParticipantJoined:
					return payloadJsonProperty.ToObject<MeetingParticipantJoinedEvent>(options) with
					{
						JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time"),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantSentToWaitingRoom:
					return payloadJsonProperty.ToObject<MeetingParticipantSentToWaitingRoomEvent>(options) with
					{
						SentToWaitingRoomOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time"),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantLeft:
					return payloadJsonProperty.ToObject<MeetingParticipantLeftEvent>(options) with
					{
						LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time"),
						LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantRoleChanged:
					return payloadJsonProperty.ToObject<MeetingParticipantRoleChangedEvent>(options) with
					{
						ChangedOn = ParseParticipantDateTime(payloadJsonProperty),
						NewRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/new_role"),
						OldRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/old_role"),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingLiveStreamStarted:
					return payloadJsonProperty.ToObject<MeetingLiveStreamStartedEvent>(options) with
					{
						StartedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/live_streaming/date_time"),
						StreamingInfo = payloadJsonProperty.GetPropertyValue<LiveStreamingInfo>("object/live_streaming")
					};
				case Models.Webhooks.EventType.MeetingLiveStreamStopped:
					return payloadJsonProperty.ToObject<MeetingLiveStreamStoppedEvent>(options) with
					{
						StoppedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/live_streaming/date_time"),
						StreamingInfo = payloadJsonProperty.GetPropertyValue<LiveStreamingInfo>("object/live_streaming")
					};
				case Models.Webhooks.EventType.RecordingArchiveFilesCompleted:
					return payloadJsonProperty.ToObject<RecordingArchiveFilesCompletedEvent>(options) with
					{
						DownloadToken = ParseDownloadToken(rootElement)
					};
				case Models.Webhooks.EventType.RecordingBatchDeleted:
					return payloadJsonProperty.ToObject<RecordingBatchDeletedEvent>(options) with
					{
						Meetings = payloadJsonProperty.GetPropertyValue<RecordingsBatch[]>("object/meetings")
					};
				case Models.Webhooks.EventType.RecordingBatchRecovered:
					return payloadJsonProperty.ToObject<RecordingBatchRecoveredEvent>(options) with
					{
						Meetings = payloadJsonProperty.GetPropertyValue<RecordingsBatch[]>("object/meetings")
					};
				case Models.Webhooks.EventType.RecordingBatchTrashed:
					return payloadJsonProperty.ToObject<RecordingBatchTrashedEvent>(options) with
					{
						MeetingUuids = payloadJsonProperty.GetPropertyValue<string[]>("object/meeting_uuids")
					};
				case Models.Webhooks.EventType.RecordingCloudStorageUsageUpdated:
					return payloadJsonProperty.ToObject<RecordingCloudStorageUsageUpdatedEvent>(options);
				case Models.Webhooks.EventType.RecordingCompleted:
					return payloadJsonProperty.ToObject<RecordingCompletedEvent>(options) with
					{
						DownloadToken = ParseDownloadToken(rootElement)
					};
				case Models.Webhooks.EventType.RecordingDeleted:
					return payloadJsonProperty.ToObject<RecordingDeletedEvent>(options);
				case Models.Webhooks.EventType.RecordingPaused:
					return ParseRecordingProgressTimestamps(payloadJsonProperty, payloadJsonProperty.ToObject<RecordingPausedEvent>(options));
				case Models.Webhooks.EventType.RecordingRecovered:
					return payloadJsonProperty.ToObject<RecordingRecoveredEvent>(options) with
					{
						DownloadToken = ParseDownloadToken(rootElement)
					};
				case Models.Webhooks.EventType.RecordingRegistrationApproved:
					return payloadJsonProperty.ToObject<RecordingRegistrationApprovedEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.RecordingRegistrationCreated:
					return payloadJsonProperty.ToObject<RecordingRegistrationCreatedEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.RecordingRegistrationDenied:
					return payloadJsonProperty.ToObject<RecordingRegistrationDeniedEvent>(options) with
					{
						Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.RecordingRenamed:
					return payloadJsonProperty.ToObject<RecordingRenamedEvent>(options) with
					{
						UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty),
						Id = payloadJsonProperty.GetPropertyValue<long>("object/id"),
						Uuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid"),
						HostId = payloadJsonProperty.GetPropertyValue<string>("object/host_id"),
						Type = payloadJsonProperty.GetPropertyValue<RecordingType>("object/type"),
						OldTitle = payloadJsonProperty.GetPropertyValue<string>("old_object/topic"),
						NewTitle = payloadJsonProperty.GetPropertyValue<string>("object/topic")
					};
				case Models.Webhooks.EventType.RecordingResumed:
					return ParseRecordingProgressTimestamps(payloadJsonProperty, payloadJsonProperty.ToObject<RecordingResumedEvent>(options));
				case Models.Webhooks.EventType.RecordingStarted:
					return ParseRecordingProgressTimestamps(payloadJsonProperty, payloadJsonProperty.ToObject<RecordingStartedEvent>(options));
				case Models.Webhooks.EventType.RecordingStopped:
					return ParseRecordingProgressTimestamps(payloadJsonProperty, payloadJsonProperty.ToObject<RecordingStoppedEvent>(options));
				case Models.Webhooks.EventType.RecordingTranscriptCompleted:
					return payloadJsonProperty.ToObject<RecordingTranscriptCompletedEvent>(options) with
					{
						DownloadToken = ParseDownloadToken(rootElement)
					};
				case Models.Webhooks.EventType.RecordingTrashed:
					return payloadJsonProperty.ToObject<RecordingTrashedEvent>(options) with
					{
						DownloadToken = ParseDownloadToken(rootElement)
					};
				case Models.Webhooks.EventType.WebinarCreated:
					return payloadJsonProperty.ToObject<WebinarCreatedEvent>(options) with
					{
						CreationSource = payloadJsonProperty.GetPropertyValue<CreationSource>("object/creation_source")
					};
				case Models.Webhooks.EventType.WebinarDeleted:
					return payloadJsonProperty.ToObject<WebinarDeletedEvent>(options);
				case Models.Webhooks.EventType.WebinarUpdated:
					var oldWebinarValues = payloadJsonProperty.GetProperty("old_object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					var newWebinarValues = payloadJsonProperty.GetProperty("object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					var webinarUpdatedEvent = payloadJsonProperty.ToObject<WebinarUpdatedEvent>(options) with
					{
						UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty),
						ModifiedFields = oldWebinarValues.Keys
							.Where(key => !oldWebinarValues[key].Equals(newWebinarValues[key]))
							.Select(key => (FieldName: key, OldValue: oldWebinarValues[key], NewValue: newWebinarValues[key]))
							.ToArray(),
						WebinarFields = oldWebinarValues.Keys
							.Where(key => oldWebinarValues[key].Equals(newWebinarValues[key]))
							.Select(key => (FieldName: key, Value: oldWebinarValues[key]))
							.ToArray()
					};

					return webinarUpdatedEvent;
				case Models.Webhooks.EventType.WebinarPermanentlyDeleted:
					return payloadJsonProperty.ToObject<WebinarPermanentlyDeletedEvent>(options);
				case Models.Webhooks.EventType.WebinarStarted:
					return payloadJsonProperty.ToObject<WebinarStartedEvent>(options);
				case Models.Webhooks.EventType.WebinarEnded:
					return payloadJsonProperty.ToObject<WebinarEndedEvent>(options) with
					{
						EndTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/end_time"),
						PracticeSession = payloadJsonProperty.GetPropertyValue("object/practice_session", false)
					};
				case Models.Webhooks.EventType.WebinarServiceIssue:
					return payloadJsonProperty.ToObject<WebinarServiceIssueEvent>(options) with
					{
						Issues = payloadJsonProperty.GetPropertyValue<string[]>("object/issues", Array.Empty<string>())
					};
				case Models.Webhooks.EventType.WebinarRecovered:
					return payloadJsonProperty.ToObject<WebinarRecoveredEvent>(options);
				case Models.Webhooks.EventType.WebinarRegistrationCreated:
					var webinarRegistrationCreatedEvent = payloadJsonProperty.ToObject<WebinarRegistrationCreatedEvent>(options);
					return ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationCreatedEvent);
				case Models.Webhooks.EventType.WebinarRegistrationApproved:
					var webinarRegistrationApprovedEvent = payloadJsonProperty.ToObject<WebinarRegistrationApprovedEvent>(options);
					return ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationApprovedEvent);
				case Models.Webhooks.EventType.WebinarRegistrationCancelled:
					var webinarRegistrationCancelledEvent = payloadJsonProperty.ToObject<WebinarRegistrationCancelledEvent>(options);
					return ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationCancelledEvent);
				case Models.Webhooks.EventType.WebinarRegistrationDenied:
					var webinarRegistrationDeniedEvent = payloadJsonProperty.ToObject<WebinarRegistrationDeniedEvent>(options);
					return ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationDeniedEvent);
				case Models.Webhooks.EventType.WebinarSharingStarted:
					return payloadJsonProperty.ToObject<WebinarSharingStartedEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty),
						ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.WebinarSharingEnded:
					return payloadJsonProperty.ToObject<WebinarSharingEndedEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty),
						ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.WebinarParticipantBind:
					return payloadJsonProperty.ToObject<WebinarParticipantBindEvent>(options) with
					{
						BindUserId = payloadJsonProperty.GetPropertyValue<string>("object/participant/bind_user_id"),
						BindParticipantUuid = payloadJsonProperty.GetPropertyValue("object/participant/bind_participant_uuid", string.Empty),
						JoinTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time"),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.WebinarParticipantFeedback:
					return payloadJsonProperty.ToObject<WebinarParticipantFeedbackEvent>(options) with
					{
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty),
						Feedback = payloadJsonProperty.GetPropertyValue<MeetingParticipantFeedback>("object/participant/feedback")
					};
				case Models.Webhooks.EventType.WebinarParticipantJoined:
					return payloadJsonProperty.ToObject<WebinarParticipantJoinedEvent>(options) with
					{
						JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time"),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.WebinarParticipantLeft:
					return payloadJsonProperty.ToObject<WebinarParticipantLeftEvent>(options) with
					{
						LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time"),
						LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.WebinarParticipantRoleChanged:
					return payloadJsonProperty.ToObject<WebinarParticipantRoleChangedEvent>(options) with
					{
						ChangedOn = ParseParticipantDateTime(payloadJsonProperty),
						NewRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/new_role"),
						OldRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/old_role"),
						Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.WebinarChatMessageFileDownloaded:
					return payloadJsonProperty.ToObject<WebinarChatMessageFileDownloadedEvent>(options) with
					{
						WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/id"),
						WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid"),
						HostAccountId = payloadJsonProperty.GetPropertyValue<string>("object/host_account_id"),
						File = payloadJsonProperty.GetPropertyValue<ChatMessageFile>("object/chat_message_file")
					};
				case Models.Webhooks.EventType.WebinarChatMessageFileSent:
					var webinarChatMessageFileSentEvent = payloadJsonProperty.ToObject<WebinarChatMessageFileSentEvent>(options) with
					{
						WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/webinar_id"),
						WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/webinar_uuid")
					};
					return ParseChatMessageFileSentEvent(payloadJsonProperty, webinarChatMessageFileSentEvent);
				case Models.Webhooks.EventType.WebinarChatMessageSent:
					var webinarChatMessageSentEvent = payloadJsonProperty.ToObject<WebinarChatMessageSentEvent>(options) with
					{
						WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/id"),
						WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid")
					};
					return ParseChatMessageSentEvent(payloadJsonProperty, webinarChatMessageSentEvent);
				case Models.Webhooks.EventType.WebinarConvertedToMeeting:
					return payloadJsonProperty.ToObject<WebinarConvertedToMeetingEvent>(options);
				case Models.Webhooks.EventType.EndpointUrlValidation:
					return payloadJsonProperty.ToObject<EndpointUrlValidationEvent>(options);
				case Models.Webhooks.EventType.MeetingParticipantJoinedBreakoutRoom:
					return payloadJsonProperty.ToObject<MeetingParticipantJoinedBreakoutRoomEvent>(options) with
					{
						Participant = ParseParticipantProperty<BreakoutRoomParticipantInfo>(payloadJsonProperty),
						JoinTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time")
					};
				case Models.Webhooks.EventType.MeetingParticipantLeftBreakoutRoom:
					return payloadJsonProperty.ToObject<MeetingParticipantLeftBreakoutRoomEvent>(options) with
					{
						Participant = ParseParticipantProperty<BreakoutRoomParticipantInfo>(payloadJsonProperty),
						LeaveTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time"),
						LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty)
					};
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingStarted:
					return payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingStartedEvent>(options) with
					{
						Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty),
						SharingDetails = ParseParticipantSharingDetails(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingEnded:
					var meetingBreakoutRoomSharingEndedEvent = payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingEndedEvent>(options);
					return meetingBreakoutRoomSharingEndedEvent with
					{
						Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty),
						SharingDetails = ParseParticipantSharingDetails(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingSummaryCompleted:
					return payloadJsonProperty.ToObject<MeetingSummaryCompletedEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryDeleted:
					return payloadJsonProperty.ToObject<MeetingSummaryDeletedEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryRecovered:
					return payloadJsonProperty.ToObject<MeetingSummaryRecoveredEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryShared:
					var meetingSummarySharedEvent = payloadJsonProperty.ToObject<MeetingSummarySharedEvent>(options);
					return meetingSummarySharedEvent with
					{
						ShareWithUsers = payloadJsonProperty.GetPropertyValue<SharedUser[]>("object/share_with_users")
					};
				case Models.Webhooks.EventType.MeetingSummaryTrashed:
					return payloadJsonProperty.ToObject<MeetingSummaryTrashedEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryUpdated:
					return payloadJsonProperty.ToObject<MeetingSummaryUpdatedEvent>(options);
				case Models.Webhooks.EventType.MeetingInvitationAccepted:
					return payloadJsonProperty.ToObject<MeetingInvitationAcceptedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingInvitationDispatched:
					return payloadJsonProperty.ToObject<MeetingInvitationDispatchedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingInvitationRejected:
					return payloadJsonProperty.ToObject<MeetingInvitationRejectedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingInvitationTimeout:
					return payloadJsonProperty.ToObject<MeetingInvitationTimeoutEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutAccepted:
					return payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutAcceptedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutMissed:
					return payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutMissedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRejected:
					return payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutRejectedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRinging:
					return payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutRingingEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutAccepted:
					return payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutAcceptedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutFailed:
					return payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutFailedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty),
						ReasonType = payloadJsonProperty.GetPropertyValue<MeetingRoomCalloutFailureReason>("object/reason_type")
					};
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutMissed:
					return payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutMissedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRejected:
					return payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutRejectedEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRinging:
					return payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutRingingEvent>(options) with
					{
						Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.MeetingAiCompanionAssetsDeleted:
					return payloadJsonProperty.ToObject<MeetingAiCompanionAssetsDeletedEvent>(options) with
					{
						DeletedAssets = payloadJsonProperty.GetPropertyValue<string[]>("object/ai_companion/deleted_assets")
					};
				case Models.Webhooks.EventType.MeetingAiCompanionStarted:
					return payloadJsonProperty.ToObject<MeetingAiCompanionStartedEvent>(options) with
					{
						Questions = payloadJsonProperty.GetPropertyValue("object/ai_companion/questions", false),
						Summary = payloadJsonProperty.GetPropertyValue("object/ai_companion/summary", false)
					};
				case Models.Webhooks.EventType.MeetingAiCompanionStopped:
					return payloadJsonProperty.ToObject<MeetingAiCompanionStoppedEvent>(options) with
					{
						Questions = payloadJsonProperty.GetPropertyValue("object/ai_companion/questions", false),
						Summary = payloadJsonProperty.GetPropertyValue("object/ai_companion/summary", false)
					};
				case Models.Webhooks.EventType.MeetingAicTranscriptCompleted:
					return payloadJsonProperty.ToObject<MeetingAicTranscriptCompletedEvent>(options);
				case Models.Webhooks.EventType.MeetingChatMessageFileDownloaded:
					return payloadJsonProperty.ToObject<MeetingChatMessageFileDownloadedEvent>(options) with
					{
						MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/id"),
						MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid"),
						HostAccountId = payloadJsonProperty.GetPropertyValue<string>("object/host_account_id"),
						File = payloadJsonProperty.GetPropertyValue<ChatMessageFile>("object/chat_message_file")
					};
				case Models.Webhooks.EventType.MeetingChatMessageFileSent:
					var meetingChatMessageFileSentEvent = payloadJsonProperty.ToObject<MeetingChatMessageFileSentEvent>(options) with
					{
						MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/meeting_id"),
						MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/meeting_uuid")
					};
					return ParseChatMessageFileSentEvent(payloadJsonProperty, meetingChatMessageFileSentEvent);
				case Models.Webhooks.EventType.MeetingChatMessageSent:
					var meetingChatMessageSentEvent = payloadJsonProperty.ToObject<MeetingChatMessageSentEvent>(options) with
					{
						MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/id"),
						MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid")
					};
					return ParseChatMessageSentEvent(payloadJsonProperty, meetingChatMessageSentEvent);
				case Models.Webhooks.EventType.MeetingConvertedToWebinar:
					return payloadJsonProperty.ToObject<MeetingConvertedToWebinarEvent>(options);
				case Models.Webhooks.EventType.MeetingDeviceTested:
					return payloadJsonProperty.ToObject<MeetingDeviceTestedEvent>(options) with
					{
						TestResult = payloadJsonProperty.GetPropertyValue<DeviceTestResult>("object/test_result")
					};
				case Models.Webhooks.EventType.MeetingRiskAlert:
					return payloadJsonProperty.ToObject<MeetingRiskAlertEvent>(options) with
					{
						ArmnDetails = payloadJsonProperty.GetPropertyValue<MeetingAtRiskDetails>("object/armn_details")
					};
				case Models.Webhooks.EventType.UserTspCreated:
					return payloadJsonProperty.ToObject<UserTspCreatedEvent>(options);
				case Models.Webhooks.EventType.UserTspDeleted:
					return payloadJsonProperty.ToObject<UserTspDeletedEvent>(options);
				case Models.Webhooks.EventType.UserTspUpdated:
					return payloadJsonProperty.ToObject<UserTspUpdatedEvent>(options) with
					{
						OldAccount = payloadJsonProperty.GetPropertyValue<TspAccount>("old_object", null)
					};
				case Models.Webhooks.EventType.PhoneCalleeAnswered:
					return payloadJsonProperty.ToObject<PhoneCalleeAnsweredEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						AnsweredOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/answer_start_time"),
						ForwardedBy = ParseForwardedBy(payloadJsonProperty),
						RedirectForwardedBy = ParseRedirectForwardedBy(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeEnded:
					return payloadJsonProperty.ToObject<PhoneCalleeEndedEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						AnsweredOn = payloadJsonProperty.GetPropertyValue<DateTime?>("object/answer_start_time", null),
						EndedOn = ParseCallEndTime(payloadJsonProperty),
						ForwardedBy = ParseForwardedBy(payloadJsonProperty),
						RedirectForwardedBy = ParseRedirectForwardedBy(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeHold:
					return payloadJsonProperty.ToObject<PhoneCalleeHoldEvent>(options) with
					{
						HoldStartedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeMeetingInviting:
					return payloadJsonProperty.ToObject<PhoneCalleeMeetingInvitingEvent>(options) with
					{
						EscalatedAt = ParseDateTime(payloadJsonProperty),
						MeetingId = payloadJsonProperty.GetPropertyValue<string>("object/meeting_id", null)
					};
				case Models.Webhooks.EventType.PhoneCalleeMissed:
					return payloadJsonProperty.ToObject<PhoneCalleeMissedEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						EndedOn = ParseCallEndTime(payloadJsonProperty),
						HandupResult = payloadJsonProperty.GetPropertyValue<string>("object/handup_result", null),
						ForwardedBy = ParseForwardedBy(payloadJsonProperty),
						RedirectForwardedBy = ParseRedirectForwardedBy(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeMute:
					return payloadJsonProperty.ToObject<PhoneCalleeMuteEvent>(options) with
					{
						MutedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeParked:
					return payloadJsonProperty.ToObject<PhoneCalleeParkedEvent>(options) with
					{
						ParkedOn = ParseDateTime(payloadJsonProperty),
						ParkCode = payloadJsonProperty.GetPropertyValue<string>("object/park_code", null),
						ParkFailureReason = payloadJsonProperty.GetPropertyValue<string>("object/park_failure_reason", null)
					};
				case Models.Webhooks.EventType.PhoneCalleeRejected:
					return payloadJsonProperty.ToObject<PhoneCalleeRejectedEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						EndedOn = ParseCallEndTime(payloadJsonProperty),
						HandupResult = payloadJsonProperty.GetPropertyValue<string>("object/handup_result", null)
					};
				case Models.Webhooks.EventType.PhoneCalleeRinging:
					return payloadJsonProperty.ToObject<PhoneCalleeRingingEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						ForwardedBy = ParseForwardedBy(payloadJsonProperty),
						RedirectForwardedBy = ParseRedirectForwardedBy(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeUnhold:
					return payloadJsonProperty.ToObject<PhoneCalleeUnholdEvent>(options) with
					{
						HoldEndedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeUnmute:
					return payloadJsonProperty.ToObject<PhoneCalleeUnmuteEvent>(options) with
					{
						UnmutedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerConnected:
					return payloadJsonProperty.ToObject<PhoneCallerConnectedEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						ConnectedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/connected_start_time")
					};
				case Models.Webhooks.EventType.PhoneCallerEnded:
					return payloadJsonProperty.ToObject<PhoneCallerEndedEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty),
						AnsweredOn = payloadJsonProperty.GetPropertyValue<DateTime?>("object/answer_start_time", null),
						EndedOn = ParseCallEndTime(payloadJsonProperty),
						HandupResult = payloadJsonProperty.GetPropertyValue<string>("object/handup_result", null)
					};
				case Models.Webhooks.EventType.PhoneCallerHold:
					return payloadJsonProperty.ToObject<PhoneCallerHoldEvent>(options) with
					{
						HoldStartedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerMeetingInviting:
					return payloadJsonProperty.ToObject<PhoneCallerMeetingInvitingEvent>(options) with
					{
						EscalatedAt = ParseDateTime(payloadJsonProperty),
						MeetingId = payloadJsonProperty.GetPropertyValue<string>("object/meeting_id", null)
					};
				case Models.Webhooks.EventType.PhoneCallerMute:
					return payloadJsonProperty.ToObject<PhoneCallerMuteEvent>(options) with
					{
						MutedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerRinging:
					return payloadJsonProperty.ToObject<PhoneCallerRingingEvent>(options) with
					{
						RingingStartedOn = ParseRingingStartTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerUnhold:
					return payloadJsonProperty.ToObject<PhoneCallerUnholdEvent>(options) with
					{
						HoldEndedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerUnmute:
					return payloadJsonProperty.ToObject<PhoneCallerUnmuteEvent>(options) with
					{
						UnmutedOn = ParseDateTime(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallElementDeleted:
					return payloadJsonProperty.ToObject<PhoneCallElementDeletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallElementIds = payloadJsonProperty.GetPropertyValue<string[]>("object/call_element_ids", []),
						DeleteAll = payloadJsonProperty.GetPropertyValue<bool>("object/delete_all", false),
						MoveToTrash = payloadJsonProperty.GetPropertyValue<bool>("object/move_to_trash", false),
						ExecuteTime = payloadJsonProperty.GetPropertyValue<DateTime?>("object/execute_time", null)
					};
				case Models.Webhooks.EventType.PhoneCallHistoryDeleted:
					return payloadJsonProperty.ToObject<PhoneCallHistoryDeletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogIds = payloadJsonProperty.GetPropertyValue<string[]>("object/call_log_ids", []),
						DeleteAll = payloadJsonProperty.GetPropertyValue<bool>("object/delete_all", false),
						MoveToTrash = payloadJsonProperty.GetPropertyValue<bool>("object/move_to_trash", false),
						ExecuteTime = payloadJsonProperty.GetPropertyValue<DateTime?>("object/execute_time", null)
					};
				case Models.Webhooks.EventType.PhoneCallLogDeleted:
					return payloadJsonProperty.ToObject<PhoneCallLogDeletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogs = ParseCallLogs<CallLogBasicInfo>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallLogPermanentlyDeleted:
					return payloadJsonProperty.ToObject<PhoneCallLogPermanentlyDeletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogs = ParseCallLogs<CallLogBasicInfo>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeCallElementCompleted:
					return payloadJsonProperty.ToObject<PhoneCalleeCallElementCompletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallElements = payloadJsonProperty.GetPropertyValue<CallElement[]>("object/call_elements")
					};
				case Models.Webhooks.EventType.PhoneCallerCallElementCompleted:
					return payloadJsonProperty.ToObject<PhoneCallerCallElementCompletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallElements = payloadJsonProperty.GetPropertyValue<CallElement[]>("object/call_elements")
					};
				case Models.Webhooks.EventType.PhoneCalleeCallHistoryCompleted:
					return payloadJsonProperty.ToObject<PhoneCalleeCallHistoryCompletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogs = ParseCallLogs<CallElement>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerCallHistoryCompleted:
					return payloadJsonProperty.ToObject<PhoneCallerCallHistoryCompletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogs = ParseCallLogs<CallElement>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCalleeCallLogCompleted:
					return payloadJsonProperty.ToObject<PhoneCalleeCallLogCompletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogs = ParseCallLogs<UserCallLog>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneCallerCallLogCompleted:
					return payloadJsonProperty.ToObject<PhoneCallerCallLogCompletedEvent>(options) with
					{
						UserId = ParseUserId(payloadJsonProperty),
						CallLogs = ParseCallLogs<UserCallLog>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneRecordingCompleted:
					return payloadJsonProperty.ToObject<PhoneRecordingCompletedEvent>(options) with
					{
						Recordings = ParseRecordings<PhoneCallRecording>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneRecordingCompletedForAccessMember:
					return payloadJsonProperty.ToObject<PhoneRecordingCompletedForAccessMemberEvent>(options) with
					{
						Recordings = ParseRecordings<PhoneCallRecording>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneRecordingDeleted:
					return payloadJsonProperty.ToObject<PhoneRecordingDeletedEvent>(options) with
					{
						Recordings = ParseRecordings<PhoneCallRecordingBasicInfo>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneRecordingFailed:
					return payloadJsonProperty.ToObject<PhoneRecordingFailedEvent>(options);
				case Models.Webhooks.EventType.PhoneRecordingPaused:
					return payloadJsonProperty.ToObject<PhoneRecordingPausedEvent>(options);
				case Models.Webhooks.EventType.PhoneRecordingPermanentlyDeleted:
					return payloadJsonProperty.ToObject<PhoneRecordingPermanentlyDeletedEvent>(options) with
					{
						Recordings = ParseRecordings<PhoneCallRecordingBasicInfo>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneRecordingResumed:
					return payloadJsonProperty.ToObject<PhoneRecordingResumedEvent>(options);
				case Models.Webhooks.EventType.PhoneRecordingStarted:
					return payloadJsonProperty.ToObject<PhoneRecordingStartedEvent>(options);
				case Models.Webhooks.EventType.PhoneRecordingStopped:
					return payloadJsonProperty.ToObject<PhoneRecordingStoppedEvent>(options);
				case Models.Webhooks.EventType.PhoneRecordingTranscriptCompleted:
					return payloadJsonProperty.ToObject<PhoneRecordingTranscriptCompletedEvent>(options) with
					{
						Recordings = ParseRecordings<PhoneCallRecording>(payloadJsonProperty)
					};
				case Models.Webhooks.EventType.PhoneSmsCampaignNumberOptIn:
					return payloadJsonProperty.ToObject<PhoneSmsCampaignNumberOptInEvent>(options);
				case Models.Webhooks.EventType.PhoneSmsCampaignNumberOptOut:
					return payloadJsonProperty.ToObject<PhoneSmsCampaignNumberOptOutEvent>(options);
				case Models.Webhooks.EventType.PhoneSmsEtiquetteBlock:
					return payloadJsonProperty.ToObject<PhoneSmsEtiquetteBlockEvent>(options);
				case Models.Webhooks.EventType.PhoneSmsEtiquetteWarn:
					return payloadJsonProperty.ToObject<PhoneSmsEtiquetteWarnEvent>(options);
				case Models.Webhooks.EventType.PhoneSmsReceived:
					return payloadJsonProperty.ToObject<PhoneSmsReceivedEvent>(options);
				case Models.Webhooks.EventType.PhoneSmsSent:
					return payloadJsonProperty.ToObject<PhoneSmsSentEvent>(options);
				case Models.Webhooks.EventType.PhoneSmsSentFailed:
					return payloadJsonProperty.ToObject<PhoneSmsSentFailedEvent>(options);
				case Models.Webhooks.EventType.PhoneVoicemailDeleted:
					return payloadJsonProperty.ToObject<PhoneVoicemailDeletedEvent>(options) with
					{
						Voicemails = payloadJsonProperty.GetPropertyValue<VoicemailBasicInfo[]>("object/voice_mails")
					};
				case Models.Webhooks.EventType.PhoneVoicemailPermanentlyDeleted:
					return payloadJsonProperty.ToObject<PhoneVoicemailPermanentlyDeletedEvent>(options) with
					{
						Voicemails = payloadJsonProperty.GetPropertyValue<VoicemailBasicInfo[]>("object/voice_mails")
					};
				case Models.Webhooks.EventType.PhoneVoicemailReceived:
					return payloadJsonProperty.ToObject<PhoneVoicemailReceivedEvent>(options);
				case Models.Webhooks.EventType.PhoneVoicemailReceivedForAccessMember:
					return payloadJsonProperty.ToObject<PhoneVoicemailReceivedForAccessMemberEvent>(options) with
					{
						AccessMemberExtensionType = payloadJsonProperty.GetPropertyValue<PhoneCallExtensionType>("object/access_member_extension_type"),
						AccessMemberId = payloadJsonProperty.GetPropertyValue<string>("object/access_member_id", null)
					};
				case Models.Webhooks.EventType.PhoneVoicemailTranscriptCompleted:
					return payloadJsonProperty.ToObject<PhoneVoicemailTranscriptCompletedEvent>(options);
				case Models.Webhooks.EventType.PhoneBlindTransferInitiated:
					return payloadJsonProperty.ToObject<PhoneBlindTransferInitiatedEvent>(options);
				case Models.Webhooks.EventType.PhoneTransferCallToVoicemailInitiated:
					return payloadJsonProperty.ToObject<PhoneTransferCallToVoicemailInitiatedEvent>(options);
				case Models.Webhooks.EventType.PhoneTransferRecipientUpdated:
					return payloadJsonProperty.ToObject<PhoneTransferRecipientUpdatedEvent>(options) with
					{
						Recipient = payloadJsonProperty.GetPropertyValue<WebhookCallTransferRecipient>("object/recipient")
					};
				case Models.Webhooks.EventType.PhoneWarmTransferCancelled:
					return payloadJsonProperty.ToObject<PhoneWarmTransferCancelledEvent>(options);
				case Models.Webhooks.EventType.PhoneWarmTransferCompleted:
					return payloadJsonProperty.ToObject<PhoneWarmTransferCompletedEvent>(options);
				case Models.Webhooks.EventType.PhoneWarmTransferInitiated:
					return payloadJsonProperty.ToObject<PhoneWarmTransferInitiatedEvent>(options) with
					{
						TransferCallId = payloadJsonProperty.GetPropertyValue<string>("object/transfer_call_id", null)
					};
				case Models.Webhooks.EventType.PhoneAiCallSummaryChanged:
					return payloadJsonProperty.ToObject<PhoneAiCallSummaryChangedEvent>(options);
				case Models.Webhooks.EventType.PhoneConferenceStarted:
					return payloadJsonProperty.ToObject<PhoneConferenceStartedEvent>(options);
				case Models.Webhooks.EventType.PhoneDeviceRegistration:
					return payloadJsonProperty.ToObject<PhoneDeviceRegistrationEvent>(options);
				case Models.Webhooks.EventType.PhoneEmergencyAlert:
					return payloadJsonProperty.ToObject<PhoneEmergencyAlertEvent>(options);
				case Models.Webhooks.EventType.PhoneGenericDeviceProvision:
					return payloadJsonProperty.ToObject<PhoneGenericDeviceProvisionEvent>(options);
				case Models.Webhooks.EventType.PhonePeeringNumberCallerIdNameUpdated:
					return payloadJsonProperty.ToObject<PhonePeeringNumberCallerIdNameUpdatedEvent>(options);
				case Models.Webhooks.EventType.PhonePeeringNumberEmergencyAddressUpdated:
					return payloadJsonProperty.ToObject<PhonePeeringNumberEmergencyAddressUpdatedEvent>(options);
				case Models.Webhooks.EventType.PhoneAccountSettingsUpdated:
					return payloadJsonProperty.ToObject<PhoneAccountSettingsUpdatedEvent>(options);
				case Models.Webhooks.EventType.PhoneGroupSettingsUpdated:
					return payloadJsonProperty.ToObject<PhoneGroupSettingsUpdatedEvent>(options);
				case Models.Webhooks.EventType.NumberManagementPeeringNumberCallerIdNameUpdated:
					return payloadJsonProperty.ToObject<NumberManagementPeeringNumberCallerIdNameUpdatedEvent>(options);
				case Models.Webhooks.EventType.NumberManagementPeeringNumberEmergencyAddressUpdated:
					return payloadJsonProperty.ToObject<NumberManagementPeeringNumberEmergencyAddressUpdatedEvent>(options);
				case Models.Webhooks.EventType.ContactCenterAssetCreated:
					return payloadJsonProperty.ToObject<ContactCenterAssetCreatedEvent>(options);
				case Models.Webhooks.EventType.ContactCenterAssetDeleted:
					return payloadJsonProperty.ToObject<ContactCenterAssetDeletedEvent>(options);
				case Models.Webhooks.EventType.ContactCenterAssetUpdated:
					return payloadJsonProperty.ToObject<ContactCenterAssetUpdatedEvent>(options);
				default:
					throw new JsonException($"{eventType} is an unknown event type");
			}
		}

		private static KeyValuePair<string, object> ConvertJsonPropertyToKeyValuePair(JsonProperty property)
		{
			var key = property.Name;
			var value = property.Value;
			switch (value.ValueKind)
			{
				case JsonValueKind.String: return new KeyValuePair<string, object>(key, value.GetString());
				case JsonValueKind.True: return new KeyValuePair<string, object>(key, true);
				case JsonValueKind.False: return new KeyValuePair<string, object>(key, false);
				case JsonValueKind.Null: return new KeyValuePair<string, object>(key, null);
				case JsonValueKind.Number:
					if (value.TryGetDecimal(out var decimalValue)) return new KeyValuePair<string, object>(key, decimalValue);
					if (value.TryGetDouble(out var doubleValue)) return new KeyValuePair<string, object>(key, doubleValue);
					if (value.TryGetSingle(out var floatValue)) return new KeyValuePair<string, object>(key, floatValue);
					if (value.TryGetInt64(out var longValue)) return new KeyValuePair<string, object>(key, longValue);
					if (value.TryGetInt32(out var intValue)) return new KeyValuePair<string, object>(key, intValue);
					if (value.TryGetInt16(out var shortValue)) return new KeyValuePair<string, object>(key, shortValue);
					throw new JsonException($"Property {key} appears to contain a numerical value but we are unable to determine the exact type");
				default:
#if NET5_0_OR_GREATER
					return new KeyValuePair<string, object>(key, value.Deserialize<object>(JsonFormatter.DefaultDeserializerOptions));
#else
					return new KeyValuePair<string, object>(key, value.GetRawText());
#endif
			}
		}

		private static T ParseParticipantProperty<T>(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<T>("object/participant");
		}

		private static T ParseRegistrantProperty<T>(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<T>("object/registrant");
		}

		private static ChatMessageFileSentEvent ParseChatMessageFileSentEvent(JsonElement payloadJsonProperty, ChatMessageFileSentEvent parsedEvent)
		{
			// All properties are under the single node but we group some properties into separate objects.
			JsonElement chatMessageFileElement = payloadJsonProperty.GetProperty("object/chat_message_file", true).Value;
			return parsedEvent with
			{
				File = chatMessageFileElement.ToObject<ChatMessageFile>(),
				Message = chatMessageFileElement.ToObject<WebhookChatMessage>(),
				Sender = ParseChatMessageSender(chatMessageFileElement),
				Recipient = ParseChatMessageRecipient(chatMessageFileElement)
			};
		}

		private static ChatMessageSentEvent ParseChatMessageSentEvent(JsonElement payloadJsonProperty, ChatMessageSentEvent parsedEvent)
		{
			// All properties are under the single node but we group some properties into separate objects.
			JsonElement chatMessageElement = payloadJsonProperty.GetProperty("object/chat_message", true).Value;
			return parsedEvent with
			{
				Message = chatMessageElement.ToObject<WebhookChatMessage>(),
				Sender = ParseChatMessageSender(chatMessageElement),
				Recipient = ParseChatMessageRecipient(chatMessageElement)
			};
		}

		/// <summary>
		/// Parse chat message sender properties.
		/// Name, session id and type are mandatory, email is optional.
		/// </summary>
		private static ChatMessageParty ParseChatMessageSender(JsonElement payloadJsonProperty)
		{
			return new ChatMessageParty
			{
				Name = payloadJsonProperty.GetPropertyValue<string>("sender_name"),
				Email = payloadJsonProperty.GetPropertyValue("sender_email", string.Empty),
				SessionId = payloadJsonProperty.GetPropertyValue<string>("sender_session_id"),
				PartyType = payloadJsonProperty.GetPropertyValue<ChatMessagePartyType>("sender_type"),
			};
		}

		/// <summary>
		/// Parse chat message recipient properties.
		/// Name, email and session id are optional, type is mandatory.
		/// </summary>
		private static ChatMessageParty ParseChatMessageRecipient(JsonElement payloadJsonProperty)
		{
			return new ChatMessageParty
			{
				Name = payloadJsonProperty.GetPropertyValue("recipient_name", string.Empty),
				Email = payloadJsonProperty.GetPropertyValue("recipient_email", string.Empty),
				SessionId = payloadJsonProperty.GetPropertyValue("recipient_session_id", string.Empty),
				PartyType = payloadJsonProperty.GetPropertyValue<ChatMessagePartyType>("recipient_type"),
			};
		}

		private static WebinarRegistrationEvent ParseWebinarRegistrationEvent(JsonElement payloadJsonProperty, WebinarRegistrationEvent parsedEvent)
		{
			return parsedEvent with
			{
				Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty),
				TrackingSource = payloadJsonProperty.GetPropertyValue<TrackingSource>("object/registrant/tracking_source", null)
			};
		}

		private static ScreenshareDetails ParseParticipantSharingDetails(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<ScreenshareDetails>("object/participant/sharing_details");
		}

		private static DateTime ParseParticipantDateTime(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
		}

		private static RecordingProgressEvent ParseRecordingProgressTimestamps(JsonElement payloadJsonProperty, RecordingProgressEvent parsedEvent)
		{
			return parsedEvent with
			{
				StartTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/recording_file/recording_start"),
				EndTime = payloadJsonProperty.GetPropertyValue<DateTime?>("object/recording_file/recording_end", null)
			};
		}

		private static string ParseDownloadToken(JsonElement rootElement)
		{
			return rootElement.GetPropertyValue("download_token", string.Empty);
		}

		private static DateTime ParseTimestampFromUnixMilliseconds(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<long>("time_stamp").FromUnixTime(UnixTimePrecision.Milliseconds);
		}

		private static DateTime ParseRingingStartTime(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<DateTime>("object/ringing_start_time");
		}

		private static DateTime ParseCallEndTime(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<DateTime>("object/call_end_time");
		}

		private static DateTime ParseDateTime(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<DateTime>("object/date_time");
		}

		private static PhoneCallParty ParseForwardedBy(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<PhoneCallParty>("object/forwarded_by", null);
		}

		private static PhoneCallParty ParseRedirectForwardedBy(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<PhoneCallParty>("object/redirect_forwarded_by", null);
		}

		private static string ParseUserId(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<string>("object/user_id");
		}

		private static T[] ParseCallLogs<T>(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<T[]>("object/call_logs");
		}

		private static T[] ParseRecordings<T>(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<T[]>("object/recordings");
		}
	}
}
