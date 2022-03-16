using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZoomNet.Models;

namespace ZoomNet.Utilities.Json
{
	/// <summary>
	/// Converts a JSON string into a <see cref="Meeting">meeting</see>.
	/// </summary>
	/// <seealso cref="JsonConverter" />
	internal class MeetingConverter : JsonConverter<Meeting>
	{
		public override Meeting Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var meetingType = rootElement.GetPropertyValue<MeetingType>("type");

			switch (meetingType)
			{
				case MeetingType.Instant:
				case MeetingType.Personal:
					return rootElement.ToObject<InstantMeeting>(options);
				case MeetingType.Scheduled:
					return rootElement.ToObject<ScheduledMeeting>(options);
				case MeetingType.RecurringFixedTime:
				case MeetingType.RecurringNoFixedTime:
					return rootElement.ToObject<RecurringMeeting>(options);
				default:
					throw new Exception($"{meetingType} is an unknown meeting type");
			}
		}

		public override void Write(Utf8JsonWriter writer, Meeting value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
