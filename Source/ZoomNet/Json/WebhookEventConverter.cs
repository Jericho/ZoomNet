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
			var payloadJsonProperty = rootElement.GetProperty("payload", true).Value;
			var eventTypeName = rootElement.GetPropertyValue("event", string.Empty);
			var eventType = Models.Webhooks.EventType.Unknown;

			if (!eventTypeName.TryToEnum(out eventType))
			{
				if (_throwWhenUnknownEventType)
				{
					throw new ArgumentException($"{eventTypeName} is not a recognized event type.", nameof(eventTypeName));
				}
				else
				{
					eventType = Models.Webhooks.EventType.Unknown;
				}
			}

			Models.Webhooks.Event webHookEvent;
			switch (eventType)
			{
				case Models.Webhooks.EventType.Unknown:
					webHookEvent = new UnknownEvent
					{
						EventType = Models.Webhooks.EventType.Unknown,
						EventTypeName = eventTypeName,
						Timestamp = timestamp,
						Payload = payloadJsonProperty
					};
					break;
				case Models.Webhooks.EventType.AppDeauthorized:
					webHookEvent = payloadJsonProperty.ToObject<AppDeauthorizedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingServiceIssue:
					var meetingServiceIssueEvent = payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(options);
					meetingServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue("object/issues", Array.Empty<string>());
					webHookEvent = meetingServiceIssueEvent;
					break;
				case Models.Webhooks.EventType.MeetingCreated:
					webHookEvent = payloadJsonProperty.ToObject<MeetingCreatedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingDeleted:
					webHookEvent = payloadJsonProperty.ToObject<MeetingDeletedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingUpdated:
					var meetingUpdatedEvent = payloadJsonProperty.ToObject<MeetingUpdatedEvent>(options);
					meetingUpdatedEvent.UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty);

					var oldMeetingValues = payloadJsonProperty.GetProperty("old_object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					var newMeetingValues = payloadJsonProperty.GetProperty("object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					meetingUpdatedEvent.ModifiedFields = oldMeetingValues.Keys
						.Where(key => !oldMeetingValues[key].Equals(newMeetingValues[key]))
						.Select(key => (FieldName: key, OldValue: oldMeetingValues[key], NewValue: newMeetingValues[key]))
						.ToArray();

					meetingUpdatedEvent.MeetingFields = oldMeetingValues.Keys
						.Where(key => oldMeetingValues[key].Equals(newMeetingValues[key]))
						.Select(key => (FieldName: key, Value: oldMeetingValues[key]))
						.ToArray();

					webHookEvent = meetingUpdatedEvent;
					break;
				case Models.Webhooks.EventType.MeetingPermanentlyDeleted:
					webHookEvent = payloadJsonProperty.ToObject<MeetingPermanentlyDeletedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingStarted:
					webHookEvent = payloadJsonProperty.ToObject<MeetingStartedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingEnded:
					var meetingEndedEvent = payloadJsonProperty.ToObject<MeetingEndedEvent>(options);
					meetingEndedEvent.EndTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/end_time");
					webHookEvent = meetingEndedEvent;
					break;
				case Models.Webhooks.EventType.MeetingRecovered:
					webHookEvent = payloadJsonProperty.ToObject<MeetingRecoveredEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingRegistrationCreated:
					var meetingRegistrationCreatedEvent = payloadJsonProperty.ToObject<MeetingRegistrationCreatedEvent>(options);
					meetingRegistrationCreatedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = meetingRegistrationCreatedEvent;
					break;
				case Models.Webhooks.EventType.MeetingRegistrationApproved:
					var meetingRegistrationApprovedEvent = payloadJsonProperty.ToObject<MeetingRegistrationApprovedEvent>(options);
					meetingRegistrationApprovedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = meetingRegistrationApprovedEvent;
					break;
				case Models.Webhooks.EventType.MeetingRegistrationCancelled:
					var meetingRegistrationCancelledEvent = payloadJsonProperty.ToObject<MeetingRegistrationCancelledEvent>(options);
					meetingRegistrationCancelledEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = meetingRegistrationCancelledEvent;
					break;
				case Models.Webhooks.EventType.MeetingRegistrationDenied:
					var meetingRegistrationDeniedEvent = payloadJsonProperty.ToObject<MeetingRegistrationDeniedEvent>(options);
					meetingRegistrationDeniedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = meetingRegistrationDeniedEvent;
					break;
				case Models.Webhooks.EventType.MeetingSharingStarted:
					var meetingSharingStartedEvent = payloadJsonProperty.ToObject<MeetingSharingStartedEvent>(options);
					meetingSharingStartedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingSharingStartedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					webHookEvent = meetingSharingStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingSharingEnded:
					var meetingSharingEndedEvent = payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(options);
					meetingSharingEndedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingSharingEndedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					webHookEvent = meetingSharingEndedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHost:
					var meetingParticipantWaitingForHostEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostEvent>(options);
					meetingParticipantWaitingForHostEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantWaitingForHostEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHostLeft:
					var meetingParticipantWaitingForHostLeftEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostLeftEvent>(options);
					meetingParticipantWaitingForHostLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantWaitingForHostLeftEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantJoinedBeforeHost:
					var meetingParticipantJoiningBeforeHostEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBeforeHostEvent>(options);
					meetingParticipantJoiningBeforeHostEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantJoiningBeforeHostEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantJoinedWaitingRoom:
					var meetingParticipantJoinedWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedWaitingRoomEvent>(options);
					meetingParticipantJoinedWaitingRoomEvent.JoinedOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantJoinedWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantJoinedWaitingRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantLeftWaitingRoom:
					var meetingParticipantLeftWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftWaitingRoomEvent>(options);
					meetingParticipantLeftWaitingRoomEvent.LeftOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantLeftWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantLeftWaitingRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantAdmitted:
					var meetingParticipantAdmittedEvent = payloadJsonProperty.ToObject<MeetingParticipantAdmittedEvent>(options);
					meetingParticipantAdmittedEvent.AdmittedOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantAdmittedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantAdmittedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantBind:
					var meetingParticipantBindEvent = payloadJsonProperty.ToObject<MeetingParticipantBindEvent>(options);
					meetingParticipantBindEvent.BindUserId = payloadJsonProperty.GetPropertyValue<string>("object/participant/bind_user_id");
					meetingParticipantBindEvent.BindParticipantUuid = payloadJsonProperty.GetPropertyValue("object/participant/bind_participant_uuid", string.Empty);
					meetingParticipantBindEvent.BoundOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantBindEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantBindEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantFeedback:
					var meetingParticipantFeedbackEvent = payloadJsonProperty.ToObject<MeetingParticipantFeedbackEvent>(options);
					meetingParticipantFeedbackEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingParticipantFeedbackEvent.Feedback = payloadJsonProperty.GetPropertyValue<MeetingParticipantFeedback>("object/participant/feedback");
					webHookEvent = meetingParticipantFeedbackEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantJoined:
					var meetingParticipantJoinedEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedEvent>(options);
					meetingParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					meetingParticipantJoinedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantJoinedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantSentToWaitingRoom:
					var meetingParticipantSentToWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantSentToWaitingRoomEvent>(options);
					meetingParticipantSentToWaitingRoomEvent.SentToWaitingRoomOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantSentToWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantSentToWaitingRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantLeft:
					var meetingParticipantLeftEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftEvent>(options);
					meetingParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					meetingParticipantLeftEvent.LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty);
					meetingParticipantLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantLeftEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantRoleChanged:
					var meetingParticipantRoleChangedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoleChangedEvent>(options);
					meetingParticipantRoleChangedEvent.ChangedOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantRoleChangedEvent.NewRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/new_role");
					meetingParticipantRoleChangedEvent.OldRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/old_role");
					meetingParticipantRoleChangedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantRoleChangedEvent;
					break;
				case Models.Webhooks.EventType.MeetingLiveStreamStarted:
					var meetingLiveStreamStartedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStartedEvent>(options);
					meetingLiveStreamStartedEvent.StartedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/live_streaming/date_time");
					meetingLiveStreamStartedEvent.StreamingInfo = payloadJsonProperty.GetPropertyValue<LiveStreamingInfo>("object/live_streaming");
					webHookEvent = meetingLiveStreamStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingLiveStreamStopped:
					var meetingLiveStreamStoppedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStoppedEvent>(options);
					meetingLiveStreamStoppedEvent.StoppedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/live_streaming/date_time");
					meetingLiveStreamStoppedEvent.StreamingInfo = payloadJsonProperty.GetPropertyValue<LiveStreamingInfo>("object/live_streaming");
					webHookEvent = meetingLiveStreamStoppedEvent;
					break;
				case Models.Webhooks.EventType.RecordingArchiveFilesCompleted:
					var recordingArchiveFilesCompletedEvent = payloadJsonProperty.ToObject<RecordingArchiveFilesCompletedEvent>(options);
					recordingArchiveFilesCompletedEvent.DownloadToken = ParseDownloadToken(rootElement);
					webHookEvent = recordingArchiveFilesCompletedEvent;
					break;
				case Models.Webhooks.EventType.RecordingBatchDeleted:
					var recordingBatchDeletedEvent = payloadJsonProperty.ToObject<RecordingBatchDeletedEvent>(options);
					recordingBatchDeletedEvent.Meetings = payloadJsonProperty.GetPropertyValue<RecordingsBatch[]>("object/meetings");
					webHookEvent = recordingBatchDeletedEvent;
					break;
				case Models.Webhooks.EventType.RecordingBatchRecovered:
					var recordingBatchRecoveredEvent = payloadJsonProperty.ToObject<RecordingBatchRecoveredEvent>(options);
					recordingBatchRecoveredEvent.Meetings = payloadJsonProperty.GetPropertyValue<RecordingsBatch[]>("object/meetings");
					webHookEvent = recordingBatchRecoveredEvent;
					break;
				case Models.Webhooks.EventType.RecordingBatchTrashed:
					var recordingBatchTrashedEvent = payloadJsonProperty.ToObject<RecordingBatchTrashedEvent>(options);
					recordingBatchTrashedEvent.MeetingUuids = payloadJsonProperty.GetPropertyValue<string[]>("object/meeting_uuids");
					webHookEvent = recordingBatchTrashedEvent;
					break;
				case Models.Webhooks.EventType.RecordingCloudStorageUsageUpdated:
					webHookEvent = payloadJsonProperty.ToObject<RecordingCloudStorageUsageUpdatedEvent>(options);
					break;
				case Models.Webhooks.EventType.RecordingCompleted:
					var recordingCompletedEvent = payloadJsonProperty.ToObject<RecordingCompletedEvent>(options);
					recordingCompletedEvent.DownloadToken = ParseDownloadToken(rootElement);
					webHookEvent = recordingCompletedEvent;
					break;
				case Models.Webhooks.EventType.RecordingDeleted:
					webHookEvent = payloadJsonProperty.ToObject<RecordingDeletedEvent>(options);
					break;
				case Models.Webhooks.EventType.RecordingPaused:
					var recordingPausedEvent = payloadJsonProperty.ToObject<RecordingPausedEvent>(options);

					ParseRecordingProgressTimestamps(payloadJsonProperty, recordingPausedEvent);

					webHookEvent = recordingPausedEvent;
					break;
				case Models.Webhooks.EventType.RecordingRecovered:
					var recordingRecoveredEvent = payloadJsonProperty.ToObject<RecordingRecoveredEvent>(options);
					recordingRecoveredEvent.DownloadToken = ParseDownloadToken(rootElement);
					webHookEvent = recordingRecoveredEvent;
					break;
				case Models.Webhooks.EventType.RecordingRegistrationApproved:
					var recordingRegistrationApproviedEvent = payloadJsonProperty.ToObject<RecordingRegistrationApprovedEvent>(options);
					recordingRegistrationApproviedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = recordingRegistrationApproviedEvent;
					break;
				case Models.Webhooks.EventType.RecordingRegistrationCreated:
					var recordingRegistrationCreatedEvent = payloadJsonProperty.ToObject<RecordingRegistrationCreatedEvent>(options);
					recordingRegistrationCreatedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = recordingRegistrationCreatedEvent;
					break;
				case Models.Webhooks.EventType.RecordingRegistrationDenied:
					var recordingRegistrationDeniedEvent = payloadJsonProperty.ToObject<RecordingRegistrationDeniedEvent>(options);
					recordingRegistrationDeniedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = recordingRegistrationDeniedEvent;
					break;
				case Models.Webhooks.EventType.RecordingRenamed:
					var recordingRenamedEvent = payloadJsonProperty.ToObject<RecordingRenamedEvent>(options);
					recordingRenamedEvent.UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty);
					recordingRenamedEvent.Id = payloadJsonProperty.GetPropertyValue<long>("object/id");
					recordingRenamedEvent.Uuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					recordingRenamedEvent.HostId = payloadJsonProperty.GetPropertyValue<string>("object/host_id");
					recordingRenamedEvent.Type = payloadJsonProperty.GetPropertyValue<RecordingType>("object/type");
					recordingRenamedEvent.OldTitle = payloadJsonProperty.GetPropertyValue<string>("old_object/topic");
					recordingRenamedEvent.NewTitle = payloadJsonProperty.GetPropertyValue<string>("object/topic");
					webHookEvent = recordingRenamedEvent;
					break;
				case Models.Webhooks.EventType.RecordingResumed:
					var recordingResumedEvent = payloadJsonProperty.ToObject<RecordingResumedEvent>(options);

					ParseRecordingProgressTimestamps(payloadJsonProperty, recordingResumedEvent);

					webHookEvent = recordingResumedEvent;
					break;
				case Models.Webhooks.EventType.RecordingStarted:
					var recordingStartedEvent = payloadJsonProperty.ToObject<RecordingStartedEvent>(options);

					ParseRecordingProgressTimestamps(payloadJsonProperty, recordingStartedEvent);

					webHookEvent = recordingStartedEvent;
					break;
				case Models.Webhooks.EventType.RecordingStopped:
					var recordingStoppedEvent = payloadJsonProperty.ToObject<RecordingStoppedEvent>(options);

					ParseRecordingProgressTimestamps(payloadJsonProperty, recordingStoppedEvent);

					webHookEvent = recordingStoppedEvent;
					break;
				case Models.Webhooks.EventType.RecordingTranscriptCompleted:
					var recordingTranscriptCompletedEvent = payloadJsonProperty.ToObject<RecordingTranscriptCompletedEvent>(options);
					recordingTranscriptCompletedEvent.DownloadToken = ParseDownloadToken(rootElement);
					webHookEvent = recordingTranscriptCompletedEvent;
					break;
				case Models.Webhooks.EventType.RecordingTrashed:
					var recordingTrashedEvent = payloadJsonProperty.ToObject<RecordingTrashedEvent>(options);
					recordingTrashedEvent.DownloadToken = ParseDownloadToken(rootElement);
					webHookEvent = recordingTrashedEvent;
					break;
				case Models.Webhooks.EventType.WebinarCreated:
					var webinarCreatedEvent = payloadJsonProperty.ToObject<WebinarCreatedEvent>(options);
					webinarCreatedEvent.CreationSource = payloadJsonProperty.GetPropertyValue<CreationSource>("object/creation_source");
					webHookEvent = webinarCreatedEvent;
					break;
				case Models.Webhooks.EventType.WebinarDeleted:
					webHookEvent = payloadJsonProperty.ToObject<WebinarDeletedEvent>(options);
					break;
				case Models.Webhooks.EventType.WebinarUpdated:
					var webinarUpdatedEvent = payloadJsonProperty.ToObject<WebinarUpdatedEvent>(options);
					webinarUpdatedEvent.UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty);

					var oldWebinarValues = payloadJsonProperty.GetProperty("old_object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					var newWebinarValues = payloadJsonProperty.GetProperty("object", true).Value
						.EnumerateObject()
						.Select(jsonProperty => ConvertJsonPropertyToKeyValuePair(jsonProperty))
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

					webinarUpdatedEvent.ModifiedFields = oldWebinarValues.Keys
						.Where(key => !oldWebinarValues[key].Equals(newWebinarValues[key]))
						.Select(key => (FieldName: key, OldValue: oldWebinarValues[key], NewValue: newWebinarValues[key]))
						.ToArray();

					webinarUpdatedEvent.WebinarFields = oldWebinarValues.Keys
						.Where(key => oldWebinarValues[key].Equals(newWebinarValues[key]))
						.Select(key => (FieldName: key, Value: oldWebinarValues[key]))
						.ToArray();

					webHookEvent = webinarUpdatedEvent;
					break;
				case Models.Webhooks.EventType.WebinarPermanentlyDeleted:
					webHookEvent = payloadJsonProperty.ToObject<WebinarPermanentlyDeletedEvent>(options);
					break;
				case Models.Webhooks.EventType.WebinarStarted:
					webHookEvent = payloadJsonProperty.ToObject<WebinarStartedEvent>(options);
					break;
				case Models.Webhooks.EventType.WebinarEnded:
					var webinarEndedEvent = payloadJsonProperty.ToObject<WebinarEndedEvent>(options);
					webinarEndedEvent.EndTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/end_time");
					webinarEndedEvent.PracticeSession = payloadJsonProperty.GetPropertyValue("object/practice_session", false);
					webHookEvent = webinarEndedEvent;
					break;
				case Models.Webhooks.EventType.WebinarServiceIssue:
					var webinarServiceIssueEvent = payloadJsonProperty.ToObject<WebinarServiceIssueEvent>(options);
					webinarServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue<string[]>("object/issues", Array.Empty<string>());
					webHookEvent = webinarServiceIssueEvent;
					break;
				case Models.Webhooks.EventType.WebinarRecovered:
					webHookEvent = payloadJsonProperty.ToObject<WebinarRecoveredEvent>(options);
					break;
				case Models.Webhooks.EventType.WebinarRegistrationCreated:
					var webinarRegistrationCreatedEvent = payloadJsonProperty.ToObject<WebinarRegistrationCreatedEvent>(options);
					ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationCreatedEvent);
					webHookEvent = webinarRegistrationCreatedEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationApproved:
					var webinarRegistrationApprovedEvent = payloadJsonProperty.ToObject<WebinarRegistrationApprovedEvent>(options);
					ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationApprovedEvent);
					webHookEvent = webinarRegistrationApprovedEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationCancelled:
					var webinarRegistrationCancelledEvent = payloadJsonProperty.ToObject<WebinarRegistrationCancelledEvent>(options);
					ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationCancelledEvent);
					webHookEvent = webinarRegistrationCancelledEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationDenied:
					var webinarRegistrationDeniedEvent = payloadJsonProperty.ToObject<WebinarRegistrationDeniedEvent>(options);
					ParseWebinarRegistrationEvent(payloadJsonProperty, webinarRegistrationDeniedEvent);
					webHookEvent = webinarRegistrationDeniedEvent;
					break;
				case Models.Webhooks.EventType.WebinarSharingStarted:
					var webinarSharingStartedEvent = payloadJsonProperty.ToObject<WebinarSharingStartedEvent>(options);
					webinarSharingStartedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarSharingStartedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					webHookEvent = webinarSharingStartedEvent;
					break;
				case Models.Webhooks.EventType.WebinarSharingEnded:
					var webinarSharingEndedEvent = payloadJsonProperty.ToObject<WebinarSharingEndedEvent>(options);
					webinarSharingEndedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarSharingEndedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					webHookEvent = webinarSharingEndedEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantBind:
					var webinarParticipantBindEvent = payloadJsonProperty.ToObject<WebinarParticipantBindEvent>(options);
					webinarParticipantBindEvent.BindUserId = payloadJsonProperty.GetPropertyValue<string>("object/participant/bind_user_id");
					webinarParticipantBindEvent.BindParticipantUuid = payloadJsonProperty.GetPropertyValue("object/participant/bind_participant_uuid", string.Empty);
					webinarParticipantBindEvent.JoinTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					webinarParticipantBindEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = webinarParticipantBindEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantFeedback:
					var webinarParticipantFeedbackEvent = payloadJsonProperty.ToObject<WebinarParticipantFeedbackEvent>(options);
					webinarParticipantFeedbackEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarParticipantFeedbackEvent.Feedback = payloadJsonProperty.GetPropertyValue<MeetingParticipantFeedback>("object/participant/feedback");
					webHookEvent = webinarParticipantFeedbackEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantJoined:
					var webinarParticipantJoinedEvent = payloadJsonProperty.ToObject<WebinarParticipantJoinedEvent>(options);
					webinarParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					webinarParticipantJoinedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = webinarParticipantJoinedEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantLeft:
					var webinarParticipantLeftEvent = payloadJsonProperty.ToObject<WebinarParticipantLeftEvent>(options);
					webinarParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					webinarParticipantLeftEvent.LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty);
					webinarParticipantLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = webinarParticipantLeftEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantRoleChanged:
					var webinarParticipantRoleChangedEvent = payloadJsonProperty.ToObject<WebinarParticipantRoleChangedEvent>(options);
					webinarParticipantRoleChangedEvent.ChangedOn = ParseParticipantDateTime(payloadJsonProperty);
					webinarParticipantRoleChangedEvent.NewRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/new_role");
					webinarParticipantRoleChangedEvent.OldRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/old_role");
					webinarParticipantRoleChangedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = webinarParticipantRoleChangedEvent;
					break;
				case Models.Webhooks.EventType.WebinarChatMessageFileDownloaded:
					var webinarChatMessageFileDownloadedEvent = payloadJsonProperty.ToObject<WebinarChatMessageFileDownloadedEvent>(options);
					webinarChatMessageFileDownloadedEvent.WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					webinarChatMessageFileDownloadedEvent.WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					webinarChatMessageFileDownloadedEvent.HostAccountId = payloadJsonProperty.GetPropertyValue<string>("object/host_account_id");
					webinarChatMessageFileDownloadedEvent.File = payloadJsonProperty.GetPropertyValue<ChatMessageFile>("object/chat_message_file");
					webHookEvent = webinarChatMessageFileDownloadedEvent;
					break;
				case Models.Webhooks.EventType.WebinarChatMessageFileSent:
					var webinarChatMessageFileSentEvent = payloadJsonProperty.ToObject<WebinarChatMessageFileSentEvent>(options);
					webinarChatMessageFileSentEvent.WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/webinar_id");
					webinarChatMessageFileSentEvent.WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/webinar_uuid");

					ParseChatMessageFileSentEvent(payloadJsonProperty, webinarChatMessageFileSentEvent);

					webHookEvent = webinarChatMessageFileSentEvent;
					break;
				case Models.Webhooks.EventType.WebinarChatMessageSent:
					var webinarChatMessageSentEvent = payloadJsonProperty.ToObject<WebinarChatMessageSentEvent>(options);
					webinarChatMessageSentEvent.WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					webinarChatMessageSentEvent.WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");

					ParseChatMessageSentEvent(payloadJsonProperty, webinarChatMessageSentEvent);

					webHookEvent = webinarChatMessageSentEvent;
					break;
				case Models.Webhooks.EventType.WebinarConvertedToMeeting:
					webHookEvent = payloadJsonProperty.ToObject<WebinarConvertedToMeetingEvent>(options);
					break;
				case Models.Webhooks.EventType.EndpointUrlValidation:
					var endpointUrlValidationEvent = payloadJsonProperty.ToObject<EndpointUrlValidationEvent>(options);
					webHookEvent = endpointUrlValidationEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantJoinedBreakoutRoom:
					var meetingParticipantJoinedBreakoutRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBreakoutRoomEvent>(options);
					meetingParticipantJoinedBreakoutRoomEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantInfo>(payloadJsonProperty);
					meetingParticipantJoinedBreakoutRoomEvent.JoinTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					webHookEvent = meetingParticipantJoinedBreakoutRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantLeftBreakoutRoom:
					var meetingParticipantLeftBreakoutRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftBreakoutRoomEvent>(options);
					meetingParticipantLeftBreakoutRoomEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantInfo>(payloadJsonProperty);
					meetingParticipantLeftBreakoutRoomEvent.LeaveTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					meetingParticipantLeftBreakoutRoomEvent.LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty);
					webHookEvent = meetingParticipantLeftBreakoutRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingStarted:
					var meetingBreakoutRoomSharingStartedEvent = payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingStartedEvent>(options);
					meetingBreakoutRoomSharingStartedEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty);
					meetingBreakoutRoomSharingStartedEvent.SharingDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					webHookEvent = meetingBreakoutRoomSharingStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingEnded:
					var meetingBreakoutRoomSharingEndedEvent = payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingEndedEvent>(options);
					meetingBreakoutRoomSharingEndedEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty);
					meetingBreakoutRoomSharingEndedEvent.SharingDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					webHookEvent = meetingBreakoutRoomSharingEndedEvent;
					break;
				case Models.Webhooks.EventType.MeetingSummaryCompleted:
					webHookEvent = payloadJsonProperty.ToObject<MeetingSummaryCompletedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingSummaryDeleted:
					webHookEvent = payloadJsonProperty.ToObject<MeetingSummaryDeletedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingSummaryRecovered:
					webHookEvent = payloadJsonProperty.ToObject<MeetingSummaryRecoveredEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingSummaryShared:
					var meetingSummarySharedEvent = payloadJsonProperty.ToObject<MeetingSummarySharedEvent>(options);
					meetingSummarySharedEvent.ShareWithUsers = payloadJsonProperty.GetPropertyValue<SharedUser[]>("object/share_with_users");
					webHookEvent = meetingSummarySharedEvent;
					break;
				case Models.Webhooks.EventType.MeetingSummaryTrashed:
					webHookEvent = payloadJsonProperty.ToObject<MeetingSummaryTrashedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingSummaryUpdated:
					webHookEvent = payloadJsonProperty.ToObject<MeetingSummaryUpdatedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingInvitationAccepted:
					var meetingInvitationAcceptedEvent = payloadJsonProperty.ToObject<MeetingInvitationAcceptedEvent>(options);
					meetingInvitationAcceptedEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					webHookEvent = meetingInvitationAcceptedEvent;
					break;
				case Models.Webhooks.EventType.MeetingInvitationDispatched:
					var meetingInvitationDispatchedEvent = payloadJsonProperty.ToObject<MeetingInvitationDispatchedEvent>(options);
					meetingInvitationDispatchedEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					webHookEvent = meetingInvitationDispatchedEvent;
					break;
				case Models.Webhooks.EventType.MeetingInvitationRejected:
					var meetingInvitationRejectedEvent = payloadJsonProperty.ToObject<MeetingInvitationRejectedEvent>(options);
					meetingInvitationRejectedEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					webHookEvent = meetingInvitationRejectedEvent;
					break;
				case Models.Webhooks.EventType.MeetingInvitationTimeout:
					var meetingInvitationTimeoutEvent = payloadJsonProperty.ToObject<MeetingInvitationTimeoutEvent>(options);
					meetingInvitationTimeoutEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					webHookEvent = meetingInvitationTimeoutEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutAccepted:
					var meetingParticipantPhoneCalloutAcceptedEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutAcceptedEvent>(options);
					meetingParticipantPhoneCalloutAcceptedEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantPhoneCalloutAcceptedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutMissed:
					var meetingParticipantPhoneCalloutMissedEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutMissedEvent>(options);
					meetingParticipantPhoneCalloutMissedEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantPhoneCalloutMissedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRejected:
					var meetingParticipantPhoneCalloutRejectedEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutRejectedEvent>(options);
					meetingParticipantPhoneCalloutRejectedEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantPhoneCalloutRejectedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRinging:
					var meetingParticipantPhoneCalloutRingingEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutRingingEvent>(options);
					meetingParticipantPhoneCalloutRingingEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantPhoneCalloutRingingEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutAccepted:
					var meetingParticipantRoomSystemCalloutAcceptedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutAcceptedEvent>(options);
					meetingParticipantRoomSystemCalloutAcceptedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantRoomSystemCalloutAcceptedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutFailed:
					var meetingParticipantRoomSystemCalloutFailedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutFailedEvent>(options);
					meetingParticipantRoomSystemCalloutFailedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					meetingParticipantRoomSystemCalloutFailedEvent.ReasonType = payloadJsonProperty.GetPropertyValue<MeetingRoomCalloutFailureReason>("object/reason_type");
					webHookEvent = meetingParticipantRoomSystemCalloutFailedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutMissed:
					var meetingParticipantRoomSystemCalloutMissedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutMissedEvent>(options);
					meetingParticipantRoomSystemCalloutMissedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantRoomSystemCalloutMissedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRejected:
					var meetingParticipantRoomSystemCalloutRejectedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutRejectedEvent>(options);
					meetingParticipantRoomSystemCalloutRejectedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantRoomSystemCalloutRejectedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRinging:
					var meetingParticipantRoomSystemCalloutRingingEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutRingingEvent>(options);
					meetingParticipantRoomSystemCalloutRingingEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantRoomSystemCalloutRingingEvent;
					break;
				case Models.Webhooks.EventType.MeetingAiCompanionAssetsDeleted:
					var meetingAiCompanionAssetsDeletedEvent = payloadJsonProperty.ToObject<MeetingAiCompanionAssetsDeletedEvent>(options);
					meetingAiCompanionAssetsDeletedEvent.DeletedAssets = payloadJsonProperty.GetPropertyValue<string[]>("object/ai_companion/deleted_assets");
					webHookEvent = meetingAiCompanionAssetsDeletedEvent;
					break;
				case Models.Webhooks.EventType.MeetingAiCompanionStarted:
					var meetingAiCompanionStartedEvent = payloadJsonProperty.ToObject<MeetingAiCompanionStartedEvent>(options);
					meetingAiCompanionStartedEvent.Questions = payloadJsonProperty.GetPropertyValue("object/ai_companion/questions", false);
					meetingAiCompanionStartedEvent.Summary = payloadJsonProperty.GetPropertyValue("object/ai_companion/summary", false);
					webHookEvent = meetingAiCompanionStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingAiCompanionStopped:
					var meetingAiCompanionStoppedEvent = payloadJsonProperty.ToObject<MeetingAiCompanionStoppedEvent>(options);
					meetingAiCompanionStoppedEvent.Questions = payloadJsonProperty.GetPropertyValue("object/ai_companion/questions", false);
					meetingAiCompanionStoppedEvent.Summary = payloadJsonProperty.GetPropertyValue("object/ai_companion/summary", false);
					webHookEvent = meetingAiCompanionStoppedEvent;
					break;
				case Models.Webhooks.EventType.MeetingAicTranscriptCompleted:
					webHookEvent = payloadJsonProperty.ToObject<MeetingAicTranscriptCompletedEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingChatMessageFileDownloaded:
					var meetingChatMessageFileDownloadedEvent = payloadJsonProperty.ToObject<MeetingChatMessageFileDownloadedEvent>(options);
					meetingChatMessageFileDownloadedEvent.MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					meetingChatMessageFileDownloadedEvent.MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					meetingChatMessageFileDownloadedEvent.HostAccountId = payloadJsonProperty.GetPropertyValue<string>("object/host_account_id");
					meetingChatMessageFileDownloadedEvent.File = payloadJsonProperty.GetPropertyValue<ChatMessageFile>("object/chat_message_file");
					webHookEvent = meetingChatMessageFileDownloadedEvent;
					break;
				case Models.Webhooks.EventType.MeetingChatMessageFileSent:
					var meetingChatMessageFileSentEvent = payloadJsonProperty.ToObject<MeetingChatMessageFileSentEvent>(options);
					meetingChatMessageFileSentEvent.MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/meeting_id");
					meetingChatMessageFileSentEvent.MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/meeting_uuid");

					ParseChatMessageFileSentEvent(payloadJsonProperty, meetingChatMessageFileSentEvent);

					webHookEvent = meetingChatMessageFileSentEvent;
					break;
				case Models.Webhooks.EventType.MeetingChatMessageSent:
					var meetingChatMessageSentEvent = payloadJsonProperty.ToObject<MeetingChatMessageSentEvent>(options);
					meetingChatMessageSentEvent.MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					meetingChatMessageSentEvent.MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");

					ParseChatMessageSentEvent(payloadJsonProperty, meetingChatMessageSentEvent);

					webHookEvent = meetingChatMessageSentEvent;
					break;
				case Models.Webhooks.EventType.MeetingConvertedToWebinar:
					webHookEvent = payloadJsonProperty.ToObject<MeetingConvertedToWebinarEvent>(options);
					break;
				case Models.Webhooks.EventType.MeetingDeviceTested:
					var meetingDeviceTestedEvent = payloadJsonProperty.ToObject<MeetingDeviceTestedEvent>(options);
					meetingDeviceTestedEvent.TestResult = payloadJsonProperty.GetPropertyValue<DeviceTestResult>("object/test_result");
					webHookEvent = meetingDeviceTestedEvent;
					break;
				case Models.Webhooks.EventType.MeetingRiskAlert:
					var meetingRiskAlertEvent = payloadJsonProperty.ToObject<MeetingRiskAlertEvent>(options);
					meetingRiskAlertEvent.ArmnDetails = payloadJsonProperty.GetPropertyValue<MeetingAtRiskDetails>("object/armn_details");
					webHookEvent = meetingRiskAlertEvent;
					break;
				default:
					throw new JsonException($"{eventType} is an unknown event type");
			}

			webHookEvent.EventType = eventType;
			webHookEvent.Timestamp = timestamp;

			return webHookEvent;
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
				default: return new KeyValuePair<string, object>(key, value.GetRawText());
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

		private static void ParseChatMessageFileSentEvent(JsonElement payloadJsonProperty, ChatMessageFileSentEvent parsedEvent)
		{
			// All properties are under the single node but we group some properties into separate objects.
			JsonElement chatMessageFileElement = payloadJsonProperty.GetProperty("object/chat_message_file", true).Value;
			parsedEvent.File = chatMessageFileElement.ToObject<ChatMessageFile>();
			parsedEvent.Message = chatMessageFileElement.ToObject<WebhookChatMessage>();
			parsedEvent.Sender = ParseChatMessageSender(chatMessageFileElement);
			parsedEvent.Recipient = ParseChatMessageRecipient(chatMessageFileElement);
		}

		private static void ParseChatMessageSentEvent(JsonElement payloadJsonProperty, ChatMessageSentEvent parsedEvent)
		{
			// All properties are under the single node but we group some properties into separate objects.
			JsonElement chatMessageElement = payloadJsonProperty.GetProperty("object/chat_message", true).Value;
			parsedEvent.Message = chatMessageElement.ToObject<WebhookChatMessage>();
			parsedEvent.Sender = ParseChatMessageSender(chatMessageElement);
			parsedEvent.Recipient = ParseChatMessageRecipient(chatMessageElement);
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

		private static void ParseWebinarRegistrationEvent(JsonElement payloadJsonProperty, WebinarRegistrationEvent parsedEvent)
		{
			parsedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
			parsedEvent.TrackingSource = payloadJsonProperty.GetPropertyValue<TrackingSource>("object/registrant/tracking_source", null);
		}

		private static ScreenshareDetails ParseParticipantSharingDetails(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<ScreenshareDetails>("object/participant/sharing_details");
		}

		private static DateTime ParseParticipantDateTime(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
		}

		private static void ParseRecordingProgressTimestamps(JsonElement payloadJsonProperty, RecordingProgressEvent parsedEvent)
		{
			parsedEvent.StartTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/recording_file/recording_start");
			parsedEvent.EndTime = payloadJsonProperty.GetPropertyValue<DateTime?>("object/recording_file/recording_end", null);
		}

		private static string ParseDownloadToken(JsonElement rootElement)
		{
			return rootElement.GetPropertyValue("download_token", string.Empty);
		}

		private static DateTime ParseTimestampFromUnixMilliseconds(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetPropertyValue<long>("time_stamp").FromUnixTime(UnixTimePrecision.Milliseconds);
		}
	}
}
