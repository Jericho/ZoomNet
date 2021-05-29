using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using static ZoomNet.Internal;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Converts a JSON string received from a webhook into and array of <see cref="Event">events</see>.
	/// </summary>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	internal class WebHookEventConverter : JsonConverter
	{
		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>
		/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Event);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.
		/// </value>
		public override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.
		/// </value>
		public override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The object value.
		/// </returns>
		/// <exception cref="System.Exception">Unable to determine the field type.</exception>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var jsonObject = JObject.Load(reader);

			jsonObject.TryGetValue("event", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty);
			jsonObject.TryGetValue("payload", StringComparison.OrdinalIgnoreCase, out JToken payloadJsonProperty);
			jsonObject.TryGetValue("event_ts", StringComparison.OrdinalIgnoreCase, out JToken timestamptJsonProperty);

			var eventType = (EventType)eventTypeJsonProperty.ToObject(typeof(EventType));

			Event webHookEvent;
			switch (eventType)
			{
				case EventType.AppDeauthorized:
					var appDeauthorizedEvent = payloadJsonProperty.ToObject<AppDeauthorizedEvent>(serializer);
					webHookEvent = appDeauthorizedEvent;
					break;
				case EventType.MeetingServiceIssue:
					var meetingServiceIssueEvent = payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(serializer);
					meetingServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue<string>("object/issues");
					webHookEvent = meetingServiceIssueEvent;
					break;
				case EventType.MeetingCreated:
					var meetingCreatedEvent = payloadJsonProperty.ToObject<MeetingCreatedEvent>(serializer);
					webHookEvent = meetingCreatedEvent;
					break;
				case EventType.MeetingDeleted:
					var meetingDeletedEvent = payloadJsonProperty.ToObject<MeetingDeletedEvent>(serializer);
					webHookEvent = meetingDeletedEvent;
					break;
				case EventType.MeetingUpdated:
					var meetingUpdatedEvent = payloadJsonProperty.ToObject<MeetingUpdatedEvent>(serializer);

					var oldMeetingValues = payloadJsonProperty.GetProperty("old_object", true).ToObject<Dictionary<string, object>>();
					var newMeetingValues = payloadJsonProperty.GetProperty("object", true).ToObject<Dictionary<string, object>>();

					meetingUpdatedEvent.ModifiedFields = oldMeetingValues.Keys
						.Select(key => (key, oldMeetingValues[key], newMeetingValues[key]))
						.ToArray();

					webHookEvent = meetingUpdatedEvent;
					break;
				case EventType.MeetingPermanentlyDeleted:
					var meetingPermanentlyDeletedEvent = payloadJsonProperty.ToObject<MeetingPermanentlyDeletedEvent>(serializer);
					webHookEvent = meetingPermanentlyDeletedEvent;
					break;
				case EventType.MeetingStarted:
					var meetingStartedEvent = payloadJsonProperty.ToObject<MeetingStartedEvent>(serializer);
					webHookEvent = meetingStartedEvent;
					break;
				case EventType.MeetingEnded:
					var meetingEndedEvent = payloadJsonProperty.ToObject<MeetingEndedEvent>(serializer);
					webHookEvent = meetingEndedEvent;
					break;
				case EventType.MeetingRecovered:
					var meetingRecoveredEvent = payloadJsonProperty.ToObject<MeetingRecoveredEvent>(serializer);
					webHookEvent = meetingRecoveredEvent;
					break;
				case EventType.MeetingRegistrationCreated:
					var meetingRegistrationCreatedEvent = payloadJsonProperty.ToObject<MeetingRegistrationCreatedEvent>(serializer);
					meetingRegistrationCreatedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = meetingRegistrationCreatedEvent;
					break;
				case EventType.MeetingRegistrationApproved:
					var meetingRegistrationApprovedEvent = payloadJsonProperty.ToObject<MeetingRegistrationApprovedEvent>(serializer);
					meetingRegistrationApprovedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = meetingRegistrationApprovedEvent;
					break;
				case EventType.MeetingRegistrationCancelled:
					var meetingRegistrationCancelledEvent = payloadJsonProperty.ToObject<MeetingRegistrationCancelledEvent>(serializer);
					meetingRegistrationCancelledEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = meetingRegistrationCancelledEvent;
					break;
				case EventType.MeetingRegistrationDenied:
					var meetingRegistrationDeniedEvent = payloadJsonProperty.ToObject<MeetingRegistrationDeniedEvent>(serializer);
					meetingRegistrationDeniedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = meetingRegistrationDeniedEvent;
					break;
				case EventType.MeetingSharingStarted:
					var meetingSharingStartedEvent = payloadJsonProperty.ToObject<MeetingSharingStartedEvent>(serializer);
					meetingSharingStartedEvent.Participant = payloadJsonProperty.GetProperty("object/participant").ToObject<WebhookParticipant>();
					meetingSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details").ToObject<ScreenshareDetails>();
					webHookEvent = meetingSharingStartedEvent;
					break;
				case EventType.MeetingSharingEnded:
					var meetingSharingEndedEvent = payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(serializer);
					meetingSharingEndedEvent.Participant = payloadJsonProperty.GetProperty("object/participant").ToObject<WebhookParticipant>();
					meetingSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details").ToObject<ScreenshareDetails>();
					webHookEvent = meetingSharingEndedEvent;
					break;
				case EventType.MeetingParticipantWaitingForHost:
					var meetingParticipantWaitingForHostEvent = payloadJsonProperty.ToObject<MeetingParticipantWaitingForHostEvent>(serializer);
					meetingParticipantWaitingForHostEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantWaitingForHostEvent;
					break;
				case EventType.MeetingParticipantJoinedBeforeHost:
					var meetingParticipantJoiningBeforeHostEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedBeforeHostEvent>(serializer);
					meetingParticipantJoiningBeforeHostEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantJoiningBeforeHostEvent;
					break;
				case EventType.MeetingParticipantJoinedWaitingRoom:
					var meetingParticipantJoinedWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedWaitingRoomEvent>(serializer);
					meetingParticipantJoinedWaitingRoomEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantJoinedWaitingRoomEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantJoinedWaitingRoomEvent;
					break;
				case EventType.MeetingParticipantLeftWaitingRoom:
					var meetingParticipantLeftWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftWaitingRoomEvent>(serializer);
					meetingParticipantLeftWaitingRoomEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantLeftWaitingRoomEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantLeftWaitingRoomEvent;
					break;
				case EventType.MeetingParticipantAdmitted:
					var meetingParticipantAdmittedEvent = payloadJsonProperty.ToObject<MeetingParticipantAdmittedEvent>(serializer);
					meetingParticipantAdmittedEvent.AdmittedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantAdmittedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantAdmittedEvent;
					break;
				case EventType.MeetingParticipantJoined:
					var meetingParticipantJoinedEvent = payloadJsonProperty.ToObject<MeetingParticipantJoinedEvent>(serializer);
					meetingParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantJoinedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantJoinedEvent;
					break;
				case EventType.MeetingParticipantSentToWaitingRoom:
					var meetingParticipantSentToWaitingRoomEvent = payloadJsonProperty.ToObject<MeetingParticipantSentToWaitingRoomEvent>(serializer);
					meetingParticipantSentToWaitingRoomEvent.SentToWaitingRoomOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					meetingParticipantSentToWaitingRoomEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantSentToWaitingRoomEvent;
					break;
				case EventType.MeetingParticipantLeft:
					var meetingParticipantLeftEvent = payloadJsonProperty.ToObject<MeetingParticipantLeftEvent>(serializer);
					meetingParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/leave_time");
					meetingParticipantLeftEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = meetingParticipantLeftEvent;
					break;
				case EventType.MeetingLiveStreamStarted:
					var meetingLiveStreamStartedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStartedEvent>(serializer);
					meetingLiveStreamStartedEvent.StartedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/date_time");
					meetingLiveStreamStartedEvent.Operator = payloadJsonProperty.GetPropertyValue<string>("object/operator");
					meetingLiveStreamStartedEvent.OperatorId = payloadJsonProperty.GetPropertyValue<string>("object/operator_id");
					meetingLiveStreamStartedEvent.StreamingInfo = payloadJsonProperty.GetProperty("object/live_streaming", true).ToObject<LiveStreamingInfo>();
					webHookEvent = meetingLiveStreamStartedEvent;
					break;
				case EventType.MeetingLiveStreamStopped:
					var meetingLiveStreamStoppedEvent = payloadJsonProperty.ToObject<MeetingLiveStreamStoppedEvent>(serializer);
					meetingLiveStreamStoppedEvent.StoppedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/date_time");
					meetingLiveStreamStoppedEvent.Operator = payloadJsonProperty.GetPropertyValue<string>("object/operator");
					meetingLiveStreamStoppedEvent.OperatorId = payloadJsonProperty.GetPropertyValue<string>("object/operator_id");
					meetingLiveStreamStoppedEvent.StreamingInfo = payloadJsonProperty.GetProperty("object/live_streaming", true).ToObject<LiveStreamingInfo>();
					webHookEvent = meetingLiveStreamStoppedEvent;
					break;
				case EventType.WebinarCreated:
					var webinarCreatedEvent = payloadJsonProperty.ToObject<WebinarCreatedEvent>(serializer);
					webHookEvent = webinarCreatedEvent;
					break;
				case EventType.WebinarDeleted:
					var webinarDeletedEvent = payloadJsonProperty.ToObject<WebinarDeletedEvent>(serializer);
					webHookEvent = webinarDeletedEvent;
					break;
				case EventType.WebinarUpdated:
					var webinarUpdatedEvent = payloadJsonProperty.ToObject<WebinarUpdatedEvent>(serializer);

					var oldWebinarValues = payloadJsonProperty.GetProperty("old_object", true).ToObject<Dictionary<string, object>>();
					var newWebinarValues = payloadJsonProperty.GetProperty("object", true).ToObject<Dictionary<string, object>>();

					webinarUpdatedEvent.ModifiedFields = oldWebinarValues.Keys
						.Select(key => (key, oldWebinarValues[key], newWebinarValues[key]))
						.ToArray();

					webHookEvent = webinarUpdatedEvent;
					break;
				case EventType.WebinarStarted:
					var webinarStartedEvent = payloadJsonProperty.ToObject<WebinarStartedEvent>(serializer);
					webHookEvent = webinarStartedEvent;
					break;
				case EventType.WebinarEnded:
					var webinarEndedEvent = payloadJsonProperty.ToObject<WebinarEndedEvent>(serializer);
					webHookEvent = webinarEndedEvent;
					break;
				case EventType.WebinarServiceIssue:
					var webinarServiceIssueEvent = payloadJsonProperty.ToObject<WebinarServiceIssueEvent>(serializer);
					webinarServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue<string>("object/issues");
					webHookEvent = webinarServiceIssueEvent;
					break;
				case EventType.WebinarRegistrationCreated:
					var webinarRegistrationCreatedEvent = payloadJsonProperty.ToObject<WebinarRegistrationCreatedEvent>(serializer);
					webinarRegistrationCreatedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = webinarRegistrationCreatedEvent;
					break;
				case EventType.WebinarRegistrationApproved:
					var webinarRegistrationApprovedEvent = payloadJsonProperty.ToObject<WebinarRegistrationApprovedEvent>(serializer);
					webinarRegistrationApprovedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = webinarRegistrationApprovedEvent;
					break;
				case EventType.WebinarRegistrationCancelled:
					var webinarRegistrationCancelledEvent = payloadJsonProperty.ToObject<WebinarRegistrationCancelledEvent>(serializer);
					webinarRegistrationCancelledEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = webinarRegistrationCancelledEvent;
					break;
				case EventType.WebinarRegistrationDenied:
					var webinarRegistrationDeniedEvent = payloadJsonProperty.ToObject<WebinarRegistrationDeniedEvent>(serializer);
					webinarRegistrationDeniedEvent.Registrant = payloadJsonProperty.GetProperty("object/registrant").ToObject<Registrant>();
					webHookEvent = webinarRegistrationDeniedEvent;
					break;
				case EventType.WebinarSharingStarted:
					var webinarSharingStartedEvent = payloadJsonProperty.ToObject<WebinarSharingStartedEvent>(serializer);
					webinarSharingStartedEvent.Participant = payloadJsonProperty.GetProperty("object/participant").ToObject<WebhookParticipant>();
					webinarSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details").ToObject<ScreenshareDetails>();
					webHookEvent = webinarSharingStartedEvent;
					break;
				case EventType.WebinarSharingEnded:
					var webinarSharingEndedEvent = payloadJsonProperty.ToObject<WebinarSharingEndedEvent>(serializer);
					webinarSharingEndedEvent.Participant = payloadJsonProperty.GetProperty("object/participant").ToObject<WebhookParticipant>();
					webinarSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant/sharing_details").ToObject<ScreenshareDetails>();
					webHookEvent = webinarSharingEndedEvent;
					break;
				case EventType.WebinarParticipantJoined:
					var webinarParticipantJoinedEvent = payloadJsonProperty.ToObject<WebinarParticipantJoinedEvent>(serializer);
					webinarParticipantJoinedEvent.JoinedOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					webinarParticipantJoinedEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = webinarParticipantJoinedEvent;
					break;
				case EventType.WebinarParticipantLeft:
					var webinarParticipantLeftEvent = payloadJsonProperty.ToObject<WebinarParticipantLeftEvent>(serializer);
					webinarParticipantLeftEvent.LeftOn = payloadJsonProperty.GetPropertyValue<DateTime>("object/participant/date_time");
					webinarParticipantLeftEvent.Participant = payloadJsonProperty.GetProperty("object/participant", true).ToObject<WebhookParticipant>();
					webHookEvent = webinarParticipantLeftEvent;
					break;
				default:
					throw new Exception($"{eventTypeJsonProperty} is an unknown event type");
			}

			webHookEvent.EventType = eventType;
			webHookEvent.Timestamp = timestamptJsonProperty.ToObject<long>().FromUnixTime(UnixTimePrecision.Milliseconds);

			return webHookEvent;
		}
	}
}
