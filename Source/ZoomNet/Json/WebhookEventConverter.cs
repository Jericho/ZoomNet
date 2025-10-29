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
					var appDeauthorizedEvent = payloadJsonProperty.ToObject<AppDeauthorizedEvent>(options);
					webHookEvent = appDeauthorizedEvent;
					break;
				case Models.Webhooks.EventType.MeetingServiceIssue:
					var meetingServiceIssueEvent = payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(options);
					meetingServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue("object/issues", Array.Empty<string>());
					webHookEvent = meetingServiceIssueEvent;
					break;
				case Models.Webhooks.EventType.MeetingCreated:
					var meetingCreatedEvent = payloadJsonProperty.ToObject<MeetingCreatedEvent>(options);
					webHookEvent = meetingCreatedEvent;
					break;
				case Models.Webhooks.EventType.MeetingDeleted:
					var meetingDeletedEvent = payloadJsonProperty.ToObject<MeetingDeletedEvent>(options);
					webHookEvent = meetingDeletedEvent;
					break;
				case Models.Webhooks.EventType.MeetingUpdated:
					var meetingUpdatedEvent = payloadJsonProperty.ToObject<MeetingUpdatedEvent>(options);

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
					var meetingPermanentlyDeletedEvent = payloadJsonProperty.ToObject<MeetingPermanentlyDeletedEvent>(options);
					webHookEvent = meetingPermanentlyDeletedEvent;
					break;
				case Models.Webhooks.EventType.MeetingStarted:
					var meetingStartedEvent = payloadJsonProperty.ToObject<MeetingStartedEvent>(options);
					webHookEvent = meetingStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingEnded:
					var meetingEndedEvent = payloadJsonProperty.ToObject<MeetingEndedEvent>(options);
					webHookEvent = meetingEndedEvent;
					break;
				case Models.Webhooks.EventType.MeetingRecovered:
					var meetingRecoveredEvent = payloadJsonProperty.ToObject<MeetingRecoveredEvent>(options);
					webHookEvent = meetingRecoveredEvent;
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
					meetingSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = meetingSharingStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingSharingEnded:
					var meetingSharingEndedEvent = payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(options);
					meetingSharingEndedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					meetingSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = meetingSharingEndedEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantWaitingForHost:
					var meetingParticipantWaitingForHostEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostEvent>(options);
					meetingParticipantWaitingForHostEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantWaitingForHostEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantJoinedBeforeHost:
					var meetingParticipantJoiningBeforeHostEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBeforeHostEvent>(options);
					meetingParticipantJoiningBeforeHostEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantJoiningBeforeHostEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantJoinedWaitingRoom:
					var meetingParticipantJoinedWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedWaitingRoomEvent>(options);
					meetingParticipantJoinedWaitingRoomEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantJoinedWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantJoinedWaitingRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantLeftWaitingRoom:
					var meetingParticipantLeftWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftWaitingRoomEvent>(options);
					meetingParticipantLeftWaitingRoomEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantLeftWaitingRoomEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantLeftWaitingRoomEvent;
					break;
				case Models.Webhooks.EventType.MeetingParticipantAdmitted:
					var meetingParticipantAdmittedEvent = payloadJsonProperty.ToObject<MeetingParticipantAdmittedEvent>(options);
					meetingParticipantAdmittedEvent.AdmittedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantAdmittedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantAdmittedEvent;
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
					meetingParticipantLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = meetingParticipantLeftEvent;
					break;
				case Models.Webhooks.EventType.MeetingLiveStreamStarted:
					var meetingLiveStreamStartedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStartedEvent>(options);
					meetingLiveStreamStartedEvent.StartedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingLiveStreamStartedEvent.Operator = payloadJsonProperty.GetPropertyValue("object/operator", string.Empty);
					meetingLiveStreamStartedEvent.OperatorId = payloadJsonProperty.GetPropertyValue("object/operator_id", string.Empty);
					meetingLiveStreamStartedEvent.StreamingInfo = payloadJsonProperty.GetProperty("object/live_streaming", true).Value.ToObject<LiveStreamingInfo>();
					webHookEvent = meetingLiveStreamStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingLiveStreamStopped:
					var meetingLiveStreamStoppedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStoppedEvent>(options);
					meetingLiveStreamStoppedEvent.StoppedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingLiveStreamStoppedEvent.Operator = payloadJsonProperty.GetPropertyValue("object/operator", string.Empty);
					meetingLiveStreamStoppedEvent.OperatorId = payloadJsonProperty.GetPropertyValue("object/operator_id", string.Empty);
					meetingLiveStreamStoppedEvent.StreamingInfo = payloadJsonProperty.GetProperty("object/live_streaming", true).Value.ToObject<LiveStreamingInfo>();
					webHookEvent = meetingLiveStreamStoppedEvent;
					break;
				case Models.Webhooks.EventType.RecordingCompleted:
					var recordingCompletedEvent = payloadJsonProperty.ToObject<RecordingCompletedEvent>(options);
					recordingCompletedEvent.DownloadToken = rootElement.GetPropertyValue<string>("download_token", string.Empty);
					recordingCompletedEvent.Recording = payloadJsonProperty.GetProperty("object", true).Value.ToObject<Recording>();
					webHookEvent = recordingCompletedEvent;
					break;
				case Models.Webhooks.EventType.WebinarCreated:
					var webinarCreatedEvent = payloadJsonProperty.ToObject<WebinarCreatedEvent>(options);
					webHookEvent = webinarCreatedEvent;
					break;
				case Models.Webhooks.EventType.WebinarDeleted:
					var webinarDeletedEvent = payloadJsonProperty.ToObject<WebinarDeletedEvent>(options);
					webHookEvent = webinarDeletedEvent;
					break;
				case Models.Webhooks.EventType.WebinarUpdated:
					var webinarUpdatedEvent = payloadJsonProperty.ToObject<WebinarUpdatedEvent>(options);

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
				case Models.Webhooks.EventType.WebinarStarted:
					var webinarStartedEvent = payloadJsonProperty.ToObject<WebinarStartedEvent>(options);
					webHookEvent = webinarStartedEvent;
					break;
				case Models.Webhooks.EventType.WebinarEnded:
					var webinarEndedEvent = payloadJsonProperty.ToObject<WebinarEndedEvent>(options);
					webHookEvent = webinarEndedEvent;
					break;
				case Models.Webhooks.EventType.WebinarServiceIssue:
					var webinarServiceIssueEvent = payloadJsonProperty.ToObject<WebinarServiceIssueEvent>(options);
					webinarServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue("object/issues", string.Empty);
					webHookEvent = webinarServiceIssueEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationCreated:
					var webinarRegistrationCreatedEvent = payloadJsonProperty.ToObject<WebinarRegistrationCreatedEvent>(options);
					webinarRegistrationCreatedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = webinarRegistrationCreatedEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationApproved:
					var webinarRegistrationApprovedEvent = payloadJsonProperty.ToObject<WebinarRegistrationApprovedEvent>(options);
					webinarRegistrationApprovedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = webinarRegistrationApprovedEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationCancelled:
					var webinarRegistrationCancelledEvent = payloadJsonProperty.ToObject<WebinarRegistrationCancelledEvent>(options);
					webinarRegistrationCancelledEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = webinarRegistrationCancelledEvent;
					break;
				case Models.Webhooks.EventType.WebinarRegistrationDenied:
					var webinarRegistrationDeniedEvent = payloadJsonProperty.ToObject<WebinarRegistrationDeniedEvent>(options);
					webinarRegistrationDeniedEvent.Registrant = ParseRegistrantProperty<Registrant>(payloadJsonProperty);
					webHookEvent = webinarRegistrationDeniedEvent;
					break;
				case Models.Webhooks.EventType.WebinarSharingStarted:
					var webinarSharingStartedEvent = payloadJsonProperty.ToObject<WebinarSharingStartedEvent>(options);
					webinarSharingStartedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = webinarSharingStartedEvent;
					break;
				case Models.Webhooks.EventType.WebinarSharingEnded:
					var webinarSharingEndedEvent = payloadJsonProperty.ToObject<WebinarSharingEndedEvent>(options);
					webinarSharingEndedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webinarSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = webinarSharingEndedEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantJoined:
					var webinarParticipantJoinedEvent = payloadJsonProperty.ToObject<WebinarParticipantJoinedEvent>(options);
					webinarParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					webinarParticipantJoinedEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = webinarParticipantJoinedEvent;
					break;
				case Models.Webhooks.EventType.WebinarParticipantLeft:
					var webinarParticipantLeftEvent = payloadJsonProperty.ToObject<WebinarParticipantLeftEvent>(options);
					webinarParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					webinarParticipantLeftEvent.Participant = ParseParticipantProperty<WebhookParticipant>(payloadJsonProperty);
					webHookEvent = webinarParticipantLeftEvent;
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
					meetingBreakoutRoomSharingStartedEvent.SharingDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = meetingBreakoutRoomSharingStartedEvent;
					break;
				case Models.Webhooks.EventType.MeetingBreakoutRoomSharingEnded:
					var meetingBreakoutRoomSharingEndedEvent = payloadJsonProperty.ToObject<MeetingBreakoutRoomSharingEndedEvent>(options);
					meetingBreakoutRoomSharingEndedEvent.Participant = ParseParticipantProperty<BreakoutRoomParticipantBasicInfo>(payloadJsonProperty);
					meetingBreakoutRoomSharingEndedEvent.SharingDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
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
					meetingSummarySharedEvent.ShareWithUsers = payloadJsonProperty.GetProperty("object/share_with_users", true).Value.ToObject<SharedUser[]>();
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
					meetingParticipantRoomSystemCalloutFailedEvent.ReasonType = payloadJsonProperty.GetProperty("object/reason_type", true).Value.ToObject<MeetingRoomCalloutFailureReason>();
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

		private T ParseParticipantProperty<T>(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<T>();
		}

		private T ParseRegistrantProperty<T>(JsonElement payloadJsonProperty)
		{
			return payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<T>();
		}
	}
}
