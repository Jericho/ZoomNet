using System;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="Meeting">meeting</see> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class MeetingConverter : ZoomNetJsonConverter<Meeting>
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
	}
}
