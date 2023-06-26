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
	/// Converts an <see cref="Event"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}"/>
	internal class WebHookEventConverter : ZoomNetJsonConverter<Event>
	{
		public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var eventType = rootElement.GetPropertyValue<string>("event").ToEnum<EventType>();
			var timestamp = rootElement.GetPropertyValue<long>("event_ts").FromUnixTime(UnixTimePrecision.Milliseconds);

			var payloadJsonProperty = rootElement.GetProperty("payload", true).Value;

			Event webHookEvent;
			switch (eventType)
			{
				case EventType.AppDeauthorized:
					var appDeauthorizedEvent = payloadJsonProperty.ToObject<AppDeauthorizedEvent>(options);
					webHookEvent = appDeauthorizedEvent;
					break;
				case EventType.MeetingServiceIssue:
					var meetingServiceIssueEvent = payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(options);
					meetingServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue("object/issues", Array.Empty<string>());
					webHookEvent = meetingServiceIssueEvent;
					break;
				case EventType.MeetingCreated:
					var meetingCreatedEvent = payloadJsonProperty.ToObject<MeetingCreatedEvent>(options);
					webHookEvent = meetingCreatedEvent;
					break;
				case EventType.MeetingDeleted:
					var meetingDeletedEvent = payloadJsonProperty.ToObject<MeetingDeletedEvent>(options);
					webHookEvent = meetingDeletedEvent;
					break;
				case EventType.MeetingUpdated:
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
				case EventType.MeetingPermanentlyDeleted:
					var meetingPermanentlyDeletedEvent = payloadJsonProperty.ToObject<MeetingPermanentlyDeletedEvent>(options);
					webHookEvent = meetingPermanentlyDeletedEvent;
					break;
				case EventType.MeetingStarted:
					var meetingStartedEvent = payloadJsonProperty.ToObject<MeetingStartedEvent>(options);
					webHookEvent = meetingStartedEvent;
					break;
				case EventType.MeetingEnded:
					var meetingEndedEvent = payloadJsonProperty.ToObject<MeetingEndedEvent>(options);
					webHookEvent = meetingEndedEvent;
					break;
				case EventType.MeetingRecovered:
					var meetingRecoveredEvent = payloadJsonProperty.ToObject<MeetingRecoveredEvent>(options);
					webHookEvent = meetingRecoveredEvent;
					break;
				case EventType.MeetingRegistrationCreated:
					var meetingRegistrationCreatedEvent = payloadJsonProperty.ToObject<MeetingRegistrationCreatedEvent>(options);
					meetingRegistrationCreatedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = meetingRegistrationCreatedEvent;
					break;
				case EventType.MeetingRegistrationApproved:
					var meetingRegistrationApprovedEvent = payloadJsonProperty.ToObject<MeetingRegistrationApprovedEvent>(options);
					meetingRegistrationApprovedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = meetingRegistrationApprovedEvent;
					break;
				case EventType.MeetingRegistrationCancelled:
					var meetingRegistrationCancelledEvent = payloadJsonProperty.ToObject<MeetingRegistrationCancelledEvent>(options);
					meetingRegistrationCancelledEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = meetingRegistrationCancelledEvent;
					break;
				case EventType.MeetingRegistrationDenied:
					var meetingRegistrationDeniedEvent = payloadJsonProperty.ToObject<MeetingRegistrationDeniedEvent>(options);
					meetingRegistrationDeniedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = meetingRegistrationDeniedEvent;
					break;
				case EventType.MeetingSharingStarted:
					var meetingSharingStartedEvent = payloadJsonProperty.ToObject<MeetingSharingStartedEvent>(options);
					meetingSharingStartedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					meetingSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = meetingSharingStartedEvent;
					break;
				case EventType.MeetingSharingEnded:
					var meetingSharingEndedEvent = payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(options);
					meetingSharingEndedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					meetingSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = meetingSharingEndedEvent;
					break;
				case EventType.MeetingParticipantWaitingForHost:
					var meetingParticipantWaitingForHostEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostEvent>(options);
					meetingParticipantWaitingForHostEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantWaitingForHostEvent;
					break;
				case EventType.MeetingParticipantJoinedBeforeHost:
					var meetingParticipantJoiningBeforeHostEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBeforeHostEvent>(options);
					meetingParticipantJoiningBeforeHostEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantJoiningBeforeHostEvent;
					break;
				case EventType.MeetingParticipantJoinedWaitingRoom:
					var meetingParticipantJoinedWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedWaitingRoomEvent>(options);
					meetingParticipantJoinedWaitingRoomEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantJoinedWaitingRoomEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantJoinedWaitingRoomEvent;
					break;
				case EventType.MeetingParticipantLeftWaitingRoom:
					var meetingParticipantLeftWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftWaitingRoomEvent>(options);
					meetingParticipantLeftWaitingRoomEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantLeftWaitingRoomEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantLeftWaitingRoomEvent;
					break;
				case EventType.MeetingParticipantAdmitted:
					var meetingParticipantAdmittedEvent = payloadJsonProperty.ToObject<MeetingParticipantAdmittedEvent>(options);
					meetingParticipantAdmittedEvent.AdmittedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantAdmittedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantAdmittedEvent;
					break;
				case EventType.MeetingParticipantJoined:
					var meetingParticipantJoinedEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedEvent>(options);
					meetingParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/join_time");
					meetingParticipantJoinedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantJoinedEvent;
					break;
				case EventType.MeetingParticipantSentToWaitingRoom:
					var meetingParticipantSentToWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantSentToWaitingRoomEvent>(options);
					meetingParticipantSentToWaitingRoomEvent.SentToWaitingRoomOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantSentToWaitingRoomEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantSentToWaitingRoomEvent;
					break;
				case EventType.MeetingParticipantLeft:
					var meetingParticipantLeftEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftEvent>(options);
					meetingParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					meetingParticipantLeftEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantLeftEvent;
					break;
				case EventType.MeetingLiveStreamStarted:
					var meetingLiveStreamStartedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStartedEvent>(options);
					meetingLiveStreamStartedEvent.StartedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingLiveStreamStartedEvent.Operator = payloadJsonProperty.GetPropertyValue("object/operator", string.Empty);
					meetingLiveStreamStartedEvent.OperatorId = payloadJsonProperty.GetPropertyValue("object/operator_id", string.Empty);
					meetingLiveStreamStartedEvent.StreamingInfo = payloadJsonProperty.GetProperty("object/live_streaming", true).Value.ToObject<LiveStreamingInfo>();
					webHookEvent = meetingLiveStreamStartedEvent;
					break;
				case EventType.MeetingLiveStreamStopped:
					var meetingLiveStreamStoppedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStoppedEvent>(options);
					meetingLiveStreamStoppedEvent.StoppedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingLiveStreamStoppedEvent.Operator = payloadJsonProperty.GetPropertyValue("object/operator", string.Empty);
					meetingLiveStreamStoppedEvent.OperatorId = payloadJsonProperty.GetPropertyValue("object/operator_id", string.Empty);
					meetingLiveStreamStoppedEvent.StreamingInfo = payloadJsonProperty.GetProperty("object/live_streaming", true).Value.ToObject<LiveStreamingInfo>();
					webHookEvent = meetingLiveStreamStoppedEvent;
					break;
				case EventType.RecordingCompleted:
					var recordingCompletedEvent = payloadJsonProperty.ToObject<RecordingCompletedEvent>(options);
					recordingCompletedEvent.DownloadToken = rootElement.GetPropertyValue<string>("download_token", string.Empty);
					recordingCompletedEvent.Recording = payloadJsonProperty.GetProperty("object", true).Value.ToObject<Recording>();
					webHookEvent = recordingCompletedEvent;
					break;
				case EventType.WebinarCreated:
					var webinarCreatedEvent = payloadJsonProperty.ToObject<WebinarCreatedEvent>(options);
					webHookEvent = webinarCreatedEvent;
					break;
				case EventType.WebinarDeleted:
					var webinarDeletedEvent = payloadJsonProperty.ToObject<WebinarDeletedEvent>(options);
					webHookEvent = webinarDeletedEvent;
					break;
				case EventType.WebinarUpdated:
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
				case EventType.WebinarStarted:
					var webinarStartedEvent = payloadJsonProperty.ToObject<WebinarStartedEvent>(options);
					webHookEvent = webinarStartedEvent;
					break;
				case EventType.WebinarEnded:
					var webinarEndedEvent = payloadJsonProperty.ToObject<WebinarEndedEvent>(options);
					webHookEvent = webinarEndedEvent;
					break;
				case EventType.WebinarServiceIssue:
					var webinarServiceIssueEvent = payloadJsonProperty.ToObject<WebinarServiceIssueEvent>(options);
					webinarServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue("object/issues", string.Empty);
					webHookEvent = webinarServiceIssueEvent;
					break;
				case EventType.WebinarRegistrationCreated:
					var webinarRegistrationCreatedEvent = payloadJsonProperty.ToObject<WebinarRegistrationCreatedEvent>(options);
					webinarRegistrationCreatedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = webinarRegistrationCreatedEvent;
					break;
				case EventType.WebinarRegistrationApproved:
					var webinarRegistrationApprovedEvent = payloadJsonProperty.ToObject<WebinarRegistrationApprovedEvent>(options);
					webinarRegistrationApprovedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = webinarRegistrationApprovedEvent;
					break;
				case EventType.WebinarRegistrationCancelled:
					var webinarRegistrationCancelledEvent = payloadJsonProperty.ToObject<WebinarRegistrationCancelledEvent>(options);
					webinarRegistrationCancelledEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = webinarRegistrationCancelledEvent;
					break;
				case EventType.WebinarRegistrationDenied:
					var webinarRegistrationDeniedEvent = payloadJsonProperty.ToObject<WebinarRegistrationDeniedEvent>(options);
					webinarRegistrationDeniedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant", true).Value.ToObject<Registrant>();
					webHookEvent = webinarRegistrationDeniedEvent;
					break;
				case EventType.WebinarSharingStarted:
					var webinarSharingStartedEvent = payloadJsonProperty.ToObject<WebinarSharingStartedEvent>(options);
					webinarSharingStartedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webinarSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = webinarSharingStartedEvent;
					break;
				case EventType.WebinarSharingEnded:
					var webinarSharingEndedEvent = payloadJsonProperty.ToObject<WebinarSharingEndedEvent>(options);
					webinarSharingEndedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webinarSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details", true).Value.ToObject<ScreenshareDetails>();
					webHookEvent = webinarSharingEndedEvent;
					break;
				case EventType.WebinarParticipantJoined:
					var webinarParticipantJoinedEvent = payloadJsonProperty.ToObject<WebinarParticipantJoinedEvent>(options);
					webinarParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					webinarParticipantJoinedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = webinarParticipantJoinedEvent;
					break;
				case EventType.WebinarParticipantLeft:
					var webinarParticipantLeftEvent = payloadJsonProperty.ToObject<WebinarParticipantLeftEvent>(options);
					webinarParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					webinarParticipantLeftEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).Value.ToObject<WebhookParticipant>();
					webHookEvent = webinarParticipantLeftEvent;
					break;
				case EventType.EndpointUrlValidation:
					var endpointUrlValidationEvent = payloadJsonProperty.ToObject<EndpointUrlValidationEvent>(options);
					webHookEvent = endpointUrlValidationEvent;
					break;
				default:
					throw new Exception($"{eventType} is an unknown event type");
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
					throw new Exception($"Property {key} appears to contain a numerical value but we are unable to determine to exact type");
				default: return new KeyValuePair<string, object>(key, value.GetRawText());
			}
		}
	}
}
