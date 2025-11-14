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

				eventType = Models.Webhooks.EventType.Unknown;
			}

			Models.Webhooks.Event webHookEvent = ParseWebhookEvent(rootElement, eventType, options);

			webHookEvent.EventType = eventType;
			webHookEvent.Timestamp = timestamp;

			return webHookEvent;
		}

		private static Models.Webhooks.Event ParseWebhookEvent(JsonElement rootElement, Models.Webhooks.EventType eventType, JsonSerializerOptions options)
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
					var meetingServiceIssueEvent = payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(options);
					meetingServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue("object/issues", Array.Empty<string>());
					return meetingServiceIssueEvent;
				case Models.Webhooks.EventType.MeetingCreated:
					return payloadJsonProperty.ToObject<MeetingCreatedEvent>(options);
				case Models.Webhooks.EventType.MeetingDeleted:
					return payloadJsonProperty.ToObject<MeetingDeletedEvent>(options);
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

					return meetingUpdatedEvent;
				case Models.Webhooks.EventType.MeetingPermanentlyDeleted:
					return payloadJsonProperty.ToObject<MeetingPermanentlyDeletedEvent>(options);
				case Models.Webhooks.EventType.MeetingStarted:
					return payloadJsonProperty.ToObject<MeetingStartedEvent>(options);
				case Models.Webhooks.EventType.MeetingEnded:
					var meetingEndedEvent = payloadJsonProperty.ToObject<MeetingEndedEvent>(options);
					meetingEndedEvent.EndTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/end_time");
					return meetingEndedEvent;
				case Models.Webhooks.EventType.MeetingRecovered:
					return payloadJsonProperty.ToObject<MeetingRecoveredEvent>(options);
				case Models.Webhooks.EventType.MeetingRegistrationCreated:
					var meetingRegistrationCreatedEvent = payloadJsonProperty.ToObject<MeetingRegistrationCreatedEvent>(options);
					meetingRegistrationCreatedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return meetingRegistrationCreatedEvent;
				case Models.Webhooks.EventType.MeetingRegistrationApproved:
					var meetingRegistrationApprovedEvent = payloadJsonProperty.ToObject<MeetingRegistrationApprovedEvent>(options);
					meetingRegistrationApprovedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return meetingRegistrationApprovedEvent;
				case Models.Webhooks.EventType.MeetingRegistrationCancelled:
					var meetingRegistrationCancelledEvent = payloadJsonProperty.ToObject<MeetingRegistrationCancelledEvent>(options);
					meetingRegistrationCancelledEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return meetingRegistrationCancelledEvent;
				case Models.Webhooks.EventType.MeetingRegistrationDenied:
					var meetingRegistrationDeniedEvent = payloadJsonProperty.ToObject<MeetingRegistrationDeniedEvent>(options);
					meetingRegistrationDeniedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return meetingRegistrationDeniedEvent;
				case Models.Webhooks.EventType.MeetingSharingStarted:
					var meetingSharingStartedEvent = payloadJsonProperty.ToObject<MeetingSharingStartedEvent>(options);
					meetingSharingStartedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingSharingStartedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					return meetingSharingStartedEvent;
				case Models.Webhooks.EventType.MeetingSharingEnded:
					var meetingSharingEndedEvent = payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(options);
					meetingSharingEndedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingSharingEndedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					return meetingSharingEndedEvent;
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHost:
					var meetingParticipantWaitingForHostEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostEvent>(options);
					meetingParticipantWaitingForHostEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantWaitingForHostEvent;
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHostLeft:
					var meetingParticipantWaitingForHostLeftEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostLeftEvent>(options);
					meetingParticipantWaitingForHostLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantWaitingForHostLeftEvent;
				case Models.Webhooks.EventType.MeetingParticipantJoinedBeforeHost:
					var meetingParticipantJoiningBeforeHostEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBeforeHostEvent>(options);
					meetingParticipantJoiningBeforeHostEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantJoiningBeforeHostEvent;
				case Models.Webhooks.EventType.MeetingParticipantJoinedWaitingRoom:
					var meetingParticipantJoinedWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedWaitingRoomEvent>(options);
					meetingParticipantJoinedWaitingRoomEvent.JoinedOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantJoinedWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantJoinedWaitingRoomEvent;
				case Models.Webhooks.EventType.MeetingParticipantLeftWaitingRoom:
					var meetingParticipantLeftWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftWaitingRoomEvent>(options);
					meetingParticipantLeftWaitingRoomEvent.LeftOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantLeftWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantLeftWaitingRoomEvent;
				case Models.Webhooks.EventType.MeetingParticipantAdmitted:
					var meetingParticipantAdmittedEvent = payloadJsonProperty.ToObject<MeetingParticipantAdmittedEvent>(options);
					meetingParticipantAdmittedEvent.AdmittedOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantAdmittedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantAdmittedEvent;
				case Models.Webhooks.EventType.MeetingParticipantBind:
					var meetingParticipantBindEvent = payloadJsonProperty.ToObject<MeetingParticipantBindEvent>(options);
					meetingParticipantBindEvent.BindUserId = payloadJsonProperty.GetPropertyValue<string>("object/participant/bind_user_id");
					meetingParticipantBindEvent.BindParticipantUuid = payloadJsonProperty.GetPropertyValue("object/participant/bind_participant_uuid", string.Empty);
					meetingParticipantBindEvent.BoundOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantBindEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantBindEvent;
				case Models.Webhooks.EventType.MeetingParticipantFeedback:
					var meetingParticipantFeedbackEvent = payloadJsonProperty.ToObject<MeetingParticipantFeedbackEvent>(options);
					meetingParticipantFeedbackEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingParticipantFeedbackEvent.Feedback = payloadJsonProperty.GetPropertyValue<MeetingParticipantFeedback>("object/participant/feedback");
					return meetingParticipantFeedbackEvent;
				case Models.Webhooks.EventType.MeetingParticipantJoined:
					var meetingParticipantJoinedEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedEvent>(options);
					meetingParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					meetingParticipantJoinedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantJoinedEvent;
				case Models.Webhooks.EventType.MeetingParticipantSentToWaitingRoom:
					var meetingParticipantSentToWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantSentToWaitingRoomEvent>(options);
					meetingParticipantSentToWaitingRoomEvent.SentToWaitingRoomOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantSentToWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantSentToWaitingRoomEvent;
				case Models.Webhooks.EventType.MeetingParticipantLeft:
					var meetingParticipantLeftEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftEvent>(options);
					meetingParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					meetingParticipantLeftEvent.LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty);
					meetingParticipantLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantLeftEvent;
				case Models.Webhooks.EventType.MeetingParticipantRoleChanged:
					var meetingParticipantRoleChangedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoleChangedEvent>(options);
					meetingParticipantRoleChangedEvent.ChangedOn = ParseParticipantDateTime(payloadJsonProperty);
					meetingParticipantRoleChangedEvent.NewRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/new_role");
					meetingParticipantRoleChangedEvent.OldRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/old_role");
					meetingParticipantRoleChangedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return meetingParticipantRoleChangedEvent;
				case Models.Webhooks.EventType.MeetingLiveStreamStarted:
					var meetingLiveStreamStartedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStartedEvent>(options);
					meetingLiveStreamStartedEvent.StartedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/live_streaming/date_time");
					meetingLiveStreamStartedEvent.StreamingInfo = payloadJsonProperty.GetPropertyValue<LiveStreamingInfo>("object/live_streaming");
					return meetingLiveStreamStartedEvent;
				case Models.Webhooks.EventType.MeetingLiveStreamStopped:
					var meetingLiveStreamStoppedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStoppedEvent>(options);
					meetingLiveStreamStoppedEvent.StoppedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/live_streaming/date_time");
					meetingLiveStreamStoppedEvent.StreamingInfo = payloadJsonProperty.GetPropertyValue<LiveStreamingInfo>("object/live_streaming");
					return meetingLiveStreamStoppedEvent;
				case Models.Webhooks.EventType.RecordingArchiveFilesCompleted:
					var recordingArchiveFilesCompletedEvent = payloadJsonProperty.ToObject<RecordingArchiveFilesCompletedEvent>(options);
					recordingArchiveFilesCompletedEvent.DownloadToken = ParseDownloadToken(rootElement);
					return recordingArchiveFilesCompletedEvent;
				case Models.Webhooks.EventType.RecordingBatchDeleted:
					var recordingBatchDeletedEvent = payloadJsonProperty.ToObject<RecordingBatchDeletedEvent>(options);
					recordingBatchDeletedEvent.Meetings = payloadJsonProperty.GetPropertyValue<RecordingsBatch[]>("object/meetings");
					return recordingBatchDeletedEvent;
				case Models.Webhooks.EventType.RecordingBatchRecovered:
					var recordingBatchRecoveredEvent = payloadJsonProperty.ToObject<RecordingBatchRecoveredEvent>(options);
					recordingBatchRecoveredEvent.Meetings = payloadJsonProperty.GetPropertyValue<RecordingsBatch[]>("object/meetings");
					return recordingBatchRecoveredEvent;
				case Models.Webhooks.EventType.RecordingBatchTrashed:
					var recordingBatchTrashedEvent = payloadJsonProperty.ToObject<RecordingBatchTrashedEvent>(options);
					recordingBatchTrashedEvent.MeetingUuids = payloadJsonProperty.GetPropertyValue<string[]>("object/meeting_uuids");
					return recordingBatchTrashedEvent;
				case Models.Webhooks.EventType.RecordingCloudStorageUsageUpdated:
					return payloadJsonProperty.ToObject<RecordingCloudStorageUsageUpdatedEvent>(options);
				case Models.Webhooks.EventType.RecordingCompleted:
					var recordingCompletedEvent = payloadJsonProperty.ToObject<RecordingCompletedEvent>(options);
					recordingCompletedEvent.DownloadToken = ParseDownloadToken(rootElement);
					return recordingCompletedEvent;
				case Models.Webhooks.EventType.RecordingDeleted:
					return payloadJsonProperty.ToObject<RecordingDeletedEvent>(options);
				case Models.Webhooks.EventType.RecordingPaused:
					var recordingPausedEvent = payloadJsonProperty.ToObject<RecordingPausedEvent>(options);
					return ParseRecordingProgressTimestamps(payloadJsonProperty, recordingPausedEvent);
				case Models.Webhooks.EventType.RecordingRecovered:
					var recordingRecoveredEvent = payloadJsonProperty.ToObject<RecordingRecoveredEvent>(options);
					recordingRecoveredEvent.DownloadToken = ParseDownloadToken(rootElement);
					return recordingRecoveredEvent;
				case Models.Webhooks.EventType.RecordingRegistrationApproved:
					var recordingRegistrationApproviedEvent = payloadJsonProperty.ToObject<RecordingRegistrationApprovedEvent>(options);
					recordingRegistrationApproviedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return recordingRegistrationApproviedEvent;
				case Models.Webhooks.EventType.RecordingRegistrationCreated:
					var recordingRegistrationCreatedEvent = payloadJsonProperty.ToObject<RecordingRegistrationCreatedEvent>(options);
					recordingRegistrationCreatedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return recordingRegistrationCreatedEvent;
				case Models.Webhooks.EventType.RecordingRegistrationDenied:
					var recordingRegistrationDeniedEvent = payloadJsonProperty.ToObject<RecordingRegistrationDeniedEvent>(options);
					recordingRegistrationDeniedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					return recordingRegistrationDeniedEvent;
				case Models.Webhooks.EventType.RecordingRenamed:
					var recordingRenamedEvent = payloadJsonProperty.ToObject<RecordingRenamedEvent>(options);
					recordingRenamedEvent.UpdatedOn = ParseTimestampFromUnixMilliseconds(payloadJsonProperty);
					recordingRenamedEvent.Id = payloadJsonProperty.GetPropertyValue<long>("object/id");
					recordingRenamedEvent.Uuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					recordingRenamedEvent.HostId = payloadJsonProperty.GetPropertyValue<string>("object/host_id");
					recordingRenamedEvent.Type = payloadJsonProperty.GetPropertyValue<RecordingType>("object/type");
					recordingRenamedEvent.OldTitle = payloadJsonProperty.GetPropertyValue<string>("old_object/topic");
					recordingRenamedEvent.NewTitle = payloadJsonProperty.GetPropertyValue<string>("object/topic");
					return recordingRenamedEvent;
				case Models.Webhooks.EventType.RecordingResumed:
					var recordingResumedEvent = payloadJsonProperty.ToObject<RecordingResumedEvent>(options);
					return ParseRecordingProgressTimestamps(payloadJsonProperty, recordingResumedEvent);
				case Models.Webhooks.EventType.RecordingStarted:
					var recordingStartedEvent = payloadJsonProperty.ToObject<RecordingStartedEvent>(options);
					return ParseRecordingProgressTimestamps(payloadJsonProperty, recordingStartedEvent);
				case Models.Webhooks.EventType.RecordingStopped:
					var recordingStoppedEvent = payloadJsonProperty.ToObject<RecordingStoppedEvent>(options);
					return ParseRecordingProgressTimestamps(payloadJsonProperty, recordingStoppedEvent);
				case Models.Webhooks.EventType.RecordingTranscriptCompleted:
					var recordingTranscriptCompletedEvent = payloadJsonProperty.ToObject<RecordingTranscriptCompletedEvent>(options);
					recordingTranscriptCompletedEvent.DownloadToken = ParseDownloadToken(rootElement);
					return recordingTranscriptCompletedEvent;
				case Models.Webhooks.EventType.RecordingTrashed:
					var recordingTrashedEvent = payloadJsonProperty.ToObject<RecordingTrashedEvent>(options);
					recordingTrashedEvent.DownloadToken = ParseDownloadToken(rootElement);
					return recordingTrashedEvent;
				case Models.Webhooks.EventType.WebinarCreated:
					var webinarCreatedEvent = payloadJsonProperty.ToObject<WebinarCreatedEvent>(options);
					webinarCreatedEvent.CreationSource = payloadJsonProperty.GetPropertyValue<CreationSource>("object/creation_source");
					return webinarCreatedEvent;
				case Models.Webhooks.EventType.WebinarDeleted:
					return payloadJsonProperty.ToObject<WebinarDeletedEvent>(options);
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

					return webinarUpdatedEvent;
				case Models.Webhooks.EventType.WebinarPermanentlyDeleted:
					return payloadJsonProperty.ToObject<WebinarPermanentlyDeletedEvent>(options);
				case Models.Webhooks.EventType.WebinarStarted:
					return payloadJsonProperty.ToObject<WebinarStartedEvent>(options);
				case Models.Webhooks.EventType.WebinarEnded:
					var webinarEndedEvent = payloadJsonProperty.ToObject<WebinarEndedEvent>(options);
					webinarEndedEvent.EndTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/end_time");
					webinarEndedEvent.PracticeSession = payloadJsonProperty.GetPropertyValue("object/practice_session", false);
					return webinarEndedEvent;
				case Models.Webhooks.EventType.WebinarServiceIssue:
					var webinarServiceIssueEvent = payloadJsonProperty.ToObject<WebinarServiceIssueEvent>(options);
					webinarServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue<string[]>("object/issues", Array.Empty<string>());
					return webinarServiceIssueEvent;
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
					var webinarSharingStartedEvent = payloadJsonProperty.ToObject<WebinarSharingStartedEvent>(options);
					webinarSharingStartedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarSharingStartedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					return webinarSharingStartedEvent;
				case Models.Webhooks.EventType.WebinarSharingEnded:
					var webinarSharingEndedEvent = payloadJsonProperty.ToObject<WebinarSharingEndedEvent>(options);
					webinarSharingEndedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarSharingEndedEvent.ScreenshareDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					return webinarSharingEndedEvent;
				case Models.Webhooks.EventType.WebinarParticipantBind:
					var webinarParticipantBindEvent = payloadJsonProperty.ToObject<WebinarParticipantBindEvent>(options);
					webinarParticipantBindEvent.BindUserId = payloadJsonProperty.GetPropertyValue<string>("object/participant/bind_user_id");
					webinarParticipantBindEvent.BindParticipantUuid = payloadJsonProperty.GetPropertyValue("object/participant/bind_participant_uuid", string.Empty);
					webinarParticipantBindEvent.JoinTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					webinarParticipantBindEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return webinarParticipantBindEvent;
				case Models.Webhooks.EventType.WebinarParticipantFeedback:
					var webinarParticipantFeedbackEvent = payloadJsonProperty.ToObject<WebinarParticipantFeedbackEvent>(options);
					webinarParticipantFeedbackEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarParticipantFeedbackEvent.Feedback = payloadJsonProperty.GetPropertyValue<MeetingParticipantFeedback>("object/participant/feedback");
					return webinarParticipantFeedbackEvent;
				case Models.Webhooks.EventType.WebinarParticipantJoined:
					var webinarParticipantJoinedEvent = payloadJsonProperty.ToObject<WebinarParticipantJoinedEvent>(options);
					webinarParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					webinarParticipantJoinedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return webinarParticipantJoinedEvent;
				case Models.Webhooks.EventType.WebinarParticipantLeft:
					var webinarParticipantLeftEvent = payloadJsonProperty.ToObject<WebinarParticipantLeftEvent>(options);
					webinarParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					webinarParticipantLeftEvent.LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty);
					webinarParticipantLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return webinarParticipantLeftEvent;
				case Models.Webhooks.EventType.WebinarParticipantRoleChanged:
					var webinarParticipantRoleChangedEvent = payloadJsonProperty.ToObject<WebinarParticipantRoleChangedEvent>(options);
					webinarParticipantRoleChangedEvent.ChangedOn = ParseParticipantDateTime(payloadJsonProperty);
					webinarParticipantRoleChangedEvent.NewRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/new_role");
					webinarParticipantRoleChangedEvent.OldRole = payloadJsonProperty.GetPropertyValue<ParticipantRole>("object/participant/old_role");
					webinarParticipantRoleChangedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					return webinarParticipantRoleChangedEvent;
				case Models.Webhooks.EventType.WebinarChatMessageFileDownloaded:
					var webinarChatMessageFileDownloadedEvent = payloadJsonProperty.ToObject<WebinarChatMessageFileDownloadedEvent>(options);
					webinarChatMessageFileDownloadedEvent.WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					webinarChatMessageFileDownloadedEvent.WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					webinarChatMessageFileDownloadedEvent.HostAccountId = payloadJsonProperty.GetPropertyValue<string>("object/host_account_id");
					webinarChatMessageFileDownloadedEvent.File = payloadJsonProperty.GetPropertyValue<ChatMessageFile>("object/chat_message_file");
					return webinarChatMessageFileDownloadedEvent;
				case Models.Webhooks.EventType.WebinarChatMessageFileSent:
					var webinarChatMessageFileSentEvent = payloadJsonProperty.ToObject<WebinarChatMessageFileSentEvent>(options);
					webinarChatMessageFileSentEvent.WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/webinar_id");
					webinarChatMessageFileSentEvent.WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/webinar_uuid");
					return ParseChatMessageFileSentEvent(payloadJsonProperty, webinarChatMessageFileSentEvent);
				case Models.Webhooks.EventType.WebinarChatMessageSent:
					var webinarChatMessageSentEvent = payloadJsonProperty.ToObject<WebinarChatMessageSentEvent>(options);
					webinarChatMessageSentEvent.WebinarId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					webinarChatMessageSentEvent.WebinarUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					return ParseChatMessageSentEvent(payloadJsonProperty, webinarChatMessageSentEvent);
				case Models.Webhooks.EventType.WebinarConvertedToMeeting:
					return payloadJsonProperty.ToObject<WebinarConvertedToMeetingEvent>(options);
				case Models.Webhooks.EventType.EndpointUrlValidation:
					return payloadJsonProperty.ToObject<EndpointUrlValidationEvent>(options);
				case Models.Webhooks.EventType.MeetingParticipantJoinedBreakoutRoom:
					var meetingParticipantJoinedBreakoutRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBreakoutRoomEvent>(options);
					meetingParticipantJoinedBreakoutRoomEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantInfo>(payloadJsonProperty);
					meetingParticipantJoinedBreakoutRoomEvent.JoinTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					return meetingParticipantJoinedBreakoutRoomEvent;
				case Models.Webhooks.EventType.MeetingParticipantLeftBreakoutRoom:
					var meetingParticipantLeftBreakoutRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftBreakoutRoomEvent>(options);
					meetingParticipantLeftBreakoutRoomEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantInfo>(payloadJsonProperty);
					meetingParticipantLeftBreakoutRoomEvent.LeaveTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					meetingParticipantLeftBreakoutRoomEvent.LeaveReason = payloadJsonProperty.GetPropertyValue("object/participant/leave_reason", string.Empty);
					return meetingParticipantLeftBreakoutRoomEvent;
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingStarted:
					var meetingBreakoutRoomSharingStartedEvent = payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingStartedEvent>(options);
					meetingBreakoutRoomSharingStartedEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty);
					meetingBreakoutRoomSharingStartedEvent.SharingDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					return meetingBreakoutRoomSharingStartedEvent;
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingEnded:
					var meetingBreakoutRoomSharingEndedEvent = payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingEndedEvent>(options);
					meetingBreakoutRoomSharingEndedEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty);
					meetingBreakoutRoomSharingEndedEvent.SharingDetails = ParseParticipantSharingDetails(payloadJsonProperty);
					return meetingBreakoutRoomSharingEndedEvent;
				case Models.Webhooks.EventType.MeetingSummaryCompleted:
					return payloadJsonProperty.ToObject<MeetingSummaryCompletedEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryDeleted:
					return payloadJsonProperty.ToObject<MeetingSummaryDeletedEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryRecovered:
					return payloadJsonProperty.ToObject<MeetingSummaryRecoveredEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryShared:
					var meetingSummarySharedEvent = payloadJsonProperty.ToObject<MeetingSummarySharedEvent>(options);
					meetingSummarySharedEvent.ShareWithUsers = payloadJsonProperty.GetPropertyValue<SharedUser[]>("object/share_with_users");
					return meetingSummarySharedEvent;
				case Models.Webhooks.EventType.MeetingSummaryTrashed:
					return payloadJsonProperty.ToObject<MeetingSummaryTrashedEvent>(options);
				case Models.Webhooks.EventType.MeetingSummaryUpdated:
					return payloadJsonProperty.ToObject<MeetingSummaryUpdatedEvent>(options);
				case Models.Webhooks.EventType.MeetingInvitationAccepted:
					var meetingInvitationAcceptedEvent = payloadJsonProperty.ToObject<MeetingInvitationAcceptedEvent>(options);
					meetingInvitationAcceptedEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					return meetingInvitationAcceptedEvent;
				case Models.Webhooks.EventType.MeetingInvitationDispatched:
					var meetingInvitationDispatchedEvent = payloadJsonProperty.ToObject<MeetingInvitationDispatchedEvent>(options);
					meetingInvitationDispatchedEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					return meetingInvitationDispatchedEvent;
				case Models.Webhooks.EventType.MeetingInvitationRejected:
					var meetingInvitationRejectedEvent = payloadJsonProperty.ToObject<MeetingInvitationRejectedEvent>(options);
					meetingInvitationRejectedEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					return meetingInvitationRejectedEvent;
				case Models.Webhooks.EventType.MeetingInvitationTimeout:
					var meetingInvitationTimeoutEvent = payloadJsonProperty.ToObject<MeetingInvitationTimeoutEvent>(options);
					meetingInvitationTimeoutEvent.Participant = ParseParticipantProperty<InvitedParticipant>(payloadJsonProperty);
					return meetingInvitationTimeoutEvent;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutAccepted:
					var meetingParticipantPhoneCalloutAcceptedEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutAcceptedEvent>(options);
					meetingParticipantPhoneCalloutAcceptedEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					return meetingParticipantPhoneCalloutAcceptedEvent;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutMissed:
					var meetingParticipantPhoneCalloutMissedEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutMissedEvent>(options);
					meetingParticipantPhoneCalloutMissedEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					return meetingParticipantPhoneCalloutMissedEvent;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRejected:
					var meetingParticipantPhoneCalloutRejectedEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutRejectedEvent>(options);
					meetingParticipantPhoneCalloutRejectedEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					return meetingParticipantPhoneCalloutRejectedEvent;
				case Models.Webhooks.EventType.MeetingParticipantPhoneCalloutRinging:
					var meetingParticipantPhoneCalloutRingingEvent = payloadJsonProperty.ToObject<MeetingParticipantPhoneCalloutRingingEvent>(options);
					meetingParticipantPhoneCalloutRingingEvent.Participant = ParseParticipantProperty<InvitedPhoneParticipant>(payloadJsonProperty);
					return meetingParticipantPhoneCalloutRingingEvent;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutAccepted:
					var meetingParticipantRoomSystemCalloutAcceptedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutAcceptedEvent>(options);
					meetingParticipantRoomSystemCalloutAcceptedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					return meetingParticipantRoomSystemCalloutAcceptedEvent;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutFailed:
					var meetingParticipantRoomSystemCalloutFailedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutFailedEvent>(options);
					meetingParticipantRoomSystemCalloutFailedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					meetingParticipantRoomSystemCalloutFailedEvent.ReasonType = payloadJsonProperty.GetPropertyValue<MeetingRoomCalloutFailureReason>("object/reason_type");
					return meetingParticipantRoomSystemCalloutFailedEvent;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutMissed:
					var meetingParticipantRoomSystemCalloutMissedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutMissedEvent>(options);
					meetingParticipantRoomSystemCalloutMissedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					return meetingParticipantRoomSystemCalloutMissedEvent;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRejected:
					var meetingParticipantRoomSystemCalloutRejectedEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutRejectedEvent>(options);
					meetingParticipantRoomSystemCalloutRejectedEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					return meetingParticipantRoomSystemCalloutRejectedEvent;
				case Models.Webhooks.EventType.MeetingParticipantRoomSystemCalloutRinging:
					var meetingParticipantRoomSystemCalloutRingingEvent = payloadJsonProperty.ToObject<MeetingParticipantRoomSystemCalloutRingingEvent>(options);
					meetingParticipantRoomSystemCalloutRingingEvent.Participant = ParseParticipantProperty<InvitedRoomParticipant>(payloadJsonProperty);
					return meetingParticipantRoomSystemCalloutRingingEvent;
				case Models.Webhooks.EventType.MeetingAiCompanionAssetsDeleted:
					var meetingAiCompanionAssetsDeletedEvent = payloadJsonProperty.ToObject<MeetingAiCompanionAssetsDeletedEvent>(options);
					meetingAiCompanionAssetsDeletedEvent.DeletedAssets = payloadJsonProperty.GetPropertyValue<string[]>("object/ai_companion/deleted_assets");
					return meetingAiCompanionAssetsDeletedEvent;
				case Models.Webhooks.EventType.MeetingAiCompanionStarted:
					var meetingAiCompanionStartedEvent = payloadJsonProperty.ToObject<MeetingAiCompanionStartedEvent>(options);
					meetingAiCompanionStartedEvent.Questions = payloadJsonProperty.GetPropertyValue("object/ai_companion/questions", false);
					meetingAiCompanionStartedEvent.Summary = payloadJsonProperty.GetPropertyValue("object/ai_companion/summary", false);
					return meetingAiCompanionStartedEvent;
				case Models.Webhooks.EventType.MeetingAiCompanionStopped:
					var meetingAiCompanionStoppedEvent = payloadJsonProperty.ToObject<MeetingAiCompanionStoppedEvent>(options);
					meetingAiCompanionStoppedEvent.Questions = payloadJsonProperty.GetPropertyValue("object/ai_companion/questions", false);
					meetingAiCompanionStoppedEvent.Summary = payloadJsonProperty.GetPropertyValue("object/ai_companion/summary", false);
					return meetingAiCompanionStoppedEvent;
				case Models.Webhooks.EventType.MeetingAicTranscriptCompleted:
					return payloadJsonProperty.ToObject<MeetingAicTranscriptCompletedEvent>(options);
				case Models.Webhooks.EventType.MeetingChatMessageFileDownloaded:
					var meetingChatMessageFileDownloadedEvent = payloadJsonProperty.ToObject<MeetingChatMessageFileDownloadedEvent>(options);
					meetingChatMessageFileDownloadedEvent.MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					meetingChatMessageFileDownloadedEvent.MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					meetingChatMessageFileDownloadedEvent.HostAccountId = payloadJsonProperty.GetPropertyValue<string>("object/host_account_id");
					meetingChatMessageFileDownloadedEvent.File = payloadJsonProperty.GetPropertyValue<ChatMessageFile>("object/chat_message_file");
					return meetingChatMessageFileDownloadedEvent;
				case Models.Webhooks.EventType.MeetingChatMessageFileSent:
					var meetingChatMessageFileSentEvent = payloadJsonProperty.ToObject<MeetingChatMessageFileSentEvent>(options);
					meetingChatMessageFileSentEvent.MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/meeting_id");
					meetingChatMessageFileSentEvent.MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/meeting_uuid");
					return ParseChatMessageFileSentEvent(payloadJsonProperty, meetingChatMessageFileSentEvent);
				case Models.Webhooks.EventType.MeetingChatMessageSent:
					var meetingChatMessageSentEvent = payloadJsonProperty.ToObject<MeetingChatMessageSentEvent>(options);
					meetingChatMessageSentEvent.MeetingId = payloadJsonProperty.GetPropertyValue<long>("object/id");
					meetingChatMessageSentEvent.MeetingUuid = payloadJsonProperty.GetPropertyValue<string>("object/uuid");
					return ParseChatMessageSentEvent(payloadJsonProperty, meetingChatMessageSentEvent);
				case Models.Webhooks.EventType.MeetingConvertedToWebinar:
					return payloadJsonProperty.ToObject<MeetingConvertedToWebinarEvent>(options);
				case Models.Webhooks.EventType.MeetingDeviceTested:
					var meetingDeviceTestedEvent = payloadJsonProperty.ToObject<MeetingDeviceTestedEvent>(options);
					meetingDeviceTestedEvent.TestResult = payloadJsonProperty.GetPropertyValue<DeviceTestResult>("object/test_result");
					return meetingDeviceTestedEvent;
				case Models.Webhooks.EventType.MeetingRiskAlert:
					var meetingRiskAlertEvent = payloadJsonProperty.ToObject<MeetingRiskAlertEvent>(options);
					meetingRiskAlertEvent.ArmnDetails = payloadJsonProperty.GetPropertyValue<MeetingAtRiskDetails>("object/armn_details");
					return meetingRiskAlertEvent;
				case Models.Webhooks.EventType.UserTspCreated:
					return payloadJsonProperty.ToObject<UserTspCreatedEvent>(options);
				case Models.Webhooks.EventType.UserTspDeleted:
					return payloadJsonProperty.ToObject<UserTspDeletedEvent>(options);
				case Models.Webhooks.EventType.UserTspUpdated:
					var userTspUpdatedEvent = payloadJsonProperty.ToObject<UserTspUpdatedEvent>(options);
					userTspUpdatedEvent.OldAccount = payloadJsonProperty.GetPropertyValue<TspAccount>("old_object", null);
					return userTspUpdatedEvent;
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

		private static ChatMessageFileSentEvent ParseChatMessageFileSentEvent(JsonElement payloadJsonProperty, ChatMessageFileSentEvent parsedEvent)
		{
			// All properties are under the single node but we group some properties into separate objects.
			JsonElement chatMessageFileElement = payloadJsonProperty.GetProperty("object/chat_message_file", true).Value;
			parsedEvent.File = chatMessageFileElement.ToObject<ChatMessageFile>();
			parsedEvent.Message = chatMessageFileElement.ToObject<WebhookChatMessage>();
			parsedEvent.Sender = ParseChatMessageSender(chatMessageFileElement);
			parsedEvent.Recipient = ParseChatMessageRecipient(chatMessageFileElement);

			return parsedEvent;
		}

		private static ChatMessageSentEvent ParseChatMessageSentEvent(JsonElement payloadJsonProperty, ChatMessageSentEvent parsedEvent)
		{
			// All properties are under the single node but we group some properties into separate objects.
			JsonElement chatMessageElement = payloadJsonProperty.GetProperty("object/chat_message", true).Value;
			parsedEvent.Message = chatMessageElement.ToObject<WebhookChatMessage>();
			parsedEvent.Sender = ParseChatMessageSender(chatMessageElement);
			parsedEvent.Recipient = ParseChatMessageRecipient(chatMessageElement);

			return parsedEvent;
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
			parsedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
			parsedEvent.TrackingSource = payloadJsonProperty.GetPropertyValue<TrackingSource>("object/registrant/tracking_source", null);

			return parsedEvent;
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
			parsedEvent.StartTime = payloadJsonProperty.GetPropertyValue<DateTime>("object/recording_file/recording_start");
			parsedEvent.EndTime = payloadJsonProperty.GetPropertyValue<DateTime?>("object/recording_file/recording_end", null);

			return parsedEvent;
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
