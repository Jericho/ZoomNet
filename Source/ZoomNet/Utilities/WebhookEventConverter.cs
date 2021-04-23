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
				case EventType.MeetingServiceIssue:
					var meetingServiceIssueEvent = payloadJsonProperty.ToObject<MeetingServiceIssueEvent>(serializer);
					meetingServiceIssueEvent.Issues = payloadJsonProperty.GetPropertyValue<string>("object/issues", true);
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

					var oldValues = payloadJsonProperty.GetProperty("old_object", true).ToObject<Dictionary<string, object>>();
					var newValues = payloadJsonProperty.GetProperty("object", true).ToObject<Dictionary<string, object>>();

					meetingUpdatedEvent.ModifiedFields = oldValues.Keys
						.Select(key => (key, oldValues[key], newValues[key]))
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
					webHookEvent = meetingRegistrationCreatedEvent;
					break;
				case EventType.MeetingRegistrationApproved:
					var meetingRegistrationApprovedEvent = payloadJsonProperty.ToObject<MeetingRegistrationApprovedEvent>(serializer);
					webHookEvent = meetingRegistrationApprovedEvent;
					break;
				case EventType.MeetingRegistrationCancelled:
					var meetingRegistrationCancelledEvent = payloadJsonProperty.ToObject<MeetingRegistrationCancelledEvent>(serializer);
					webHookEvent = meetingRegistrationCancelledEvent;
					break;
				case EventType.MeetingRegistrationDenied:
					var meetingRegistrationDeniedEvent = payloadJsonProperty.ToObject<MeetingRegistrationDeniedEvent>(serializer);
					webHookEvent = meetingRegistrationDeniedEvent;
					break;
				case EventType.MeetingSharingStarted:
					var meetingSharingStartedEvent = payloadJsonProperty.ToObject<MeetingSharingStartedEvent>(serializer);
					meetingSharingStartedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant").ToObject<ScreenshareSharingDetails>();
					webHookEvent = meetingSharingStartedEvent;
					break;
				case EventType.MeetingSharingEnded:
					var meetingSharingEndedEvent = payloadJsonProperty.ToObject<MeetingSharingEndedEvent>(serializer);
					meetingSharingEndedEvent.ScreenshareDetails = payloadJsonProperty.GetProperty("object/participant").ToObject<ScreenshareSharingDetails>();
					webHookEvent = meetingSharingEndedEvent;
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
