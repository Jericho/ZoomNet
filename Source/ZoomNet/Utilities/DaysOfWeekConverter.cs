using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Converts a JSON string into and array of <see cref="DayOfWeek">days of the week</see>.
	/// </summary>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	internal class DaysOfWeekConverter : JsonConverter
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
			return objectType == typeof(DayOfWeek) || objectType == typeof(DayOfWeek[]);
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
			get { return true; }
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			/*
				IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
				but the values expected by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			*/
			switch (value)
			{
				case DayOfWeek dayOfWeek:
					var singleDay = (Convert.ToInt32(dayOfWeek) + 1).ToString();
					serializer.Serialize(writer, singleDay);
					break;
				case DayOfWeek[] daysOfWeek:
					var multipleDays = string.Join(",", daysOfWeek.Select(day => (Convert.ToInt32(day) + 1).ToString()));
					serializer.Serialize(writer, multipleDays);
					break;
				default:
					throw new Exception("Unable to serialize the value");
			}
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
			/*
				IMPORTANT: the values in System.DayOfWeek start at zero (i.e.: Sunday=0, Monday=1, ..., Saturday=6)
				but the values returned by the Zoom API start at one (i.e.:  Sunday=1, Monday=2, ..., Saturday=7).
			*/

			var rawValue = reader.Value as string;

			if (objectType == typeof(DayOfWeek))
			{
				var value = Convert.ToInt32(rawValue) - 1;
				return (DayOfWeek)value;
			}
			else if (objectType == typeof(DayOfWeek[]))
			{
				var values = rawValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				return values
					.Select(value => Convert.ToInt32(value) - 1)
					.Select(value => (DayOfWeek)value)
					.ToArray();
			}

			JValue jValue = new JValue(reader.Value);
			switch (reader.TokenType)
			{
				//case JsonToken.Integer:
				//	value = Convert.ToInt32(reader.Value);
				//	break;
				//case JsonToken.Float:
				//	value = Convert.ToDecimal(reader.Value);
				//	break;
				//case JsonToken.String:
				//	value = Convert.ToString(reader.Value);
				//	break;
				//case JsonToken.Boolean:
				//	value = Convert.ToBoolean(reader.Value);
				//	break;
				//case JsonToken.Null:
				//	value = null;
				//	break;
				//case JsonToken.Date:
				//	value = Convert.ToDateTime(reader.Value);
				//	break;
				//case JsonToken.Bytes:
				//	value = Convert.ToByte(reader.Value);
				//	break;
				default:
					Console.WriteLine("Default case");
					Console.WriteLine(reader.TokenType.ToString());
					break;
			}

			//if (reader.TokenType == JsonToken.String)
			//{
			//	var jValue = JV JArray.Load(reader);

			//	if (objectType == typeof(DayOfWeek))
			//	{
			//	}
			//	else if objectType == typeof(DayOfWeek[])
			//	{
			//	}
			//	case DayOfWeek dayOfWeek:
			//		var singleDay = (Convert.ToInt32(dayOfWeek) + 1).ToString();
			//	serializer.Serialize(writer, singleDay);
			//	break;
			//	case DayOfWeek[] daysOfWeek:
			//		var multipleDays = string.Join(",", daysOfWeek.Select(day => (Convert.ToInt32(day) + 1).ToString()));
			//	serializer.Serialize(writer, multipleDays);
			//	break;
			//	default:
			//		throw new Exception("Unable to serialize the value");
			//}

			//if (reader.TokenType == JsonToken.String)
			//{
			//	var jArray = JArray.Load(reader);
			//	var items = jArray
			//		.OfType<JObject>()
			//		.Select(item => Convert(item, serializer))
			//		.Where(item => item != null)
			//		.ToArray();

			//	return items;
			//}
			//else if (reader.TokenType == JsonToken.StartObject)
			//{
			//	var jObject = JObject.Load(reader);
			//	return Convert(jObject, serializer);
			//}

			throw new Exception("Unable to deserialize the value");
		}

		//private Event Convert(JObject jsonObject, JsonSerializer serializer)
		//{
		//	jsonObject.TryGetValue("event_name", StringComparison.OrdinalIgnoreCase, out JToken eventTypeJsonProperty);
		//	var eventType = (EventType)eventTypeJsonProperty.ToObject(typeof(EventType));

		//	var emailActivityEvent = (Event)null;
		//	switch (eventType)
		//	{
		//		case EventType.Bounce:
		//			emailActivityEvent = jsonObject.ToObject<BounceEvent>(serializer);
		//			break;
		//		case EventType.Open:
		//			emailActivityEvent = jsonObject.ToObject<OpenEvent>(serializer);
		//			break;
		//		case EventType.Click:
		//			emailActivityEvent = jsonObject.ToObject<ClickEvent>(serializer);
		//			break;
		//		case EventType.Processed:
		//			emailActivityEvent = jsonObject.ToObject<ProcessedEvent>(serializer);
		//			break;
		//		case EventType.Dropped:
		//			emailActivityEvent = jsonObject.ToObject<DroppedEvent>(serializer);
		//			break;
		//		case EventType.Delivered:
		//			emailActivityEvent = jsonObject.ToObject<DeliveredEvent>(serializer);
		//			break;
		//		case EventType.Deferred:
		//			emailActivityEvent = jsonObject.ToObject<DeferredEvent>(serializer);
		//			break;
		//		case EventType.SpamReport:
		//			emailActivityEvent = jsonObject.ToObject<SpamReportEvent>(serializer);
		//			break;
		//		case EventType.Unsubscribe:
		//			emailActivityEvent = jsonObject.ToObject<UnsubscribeEvent>(serializer);
		//			break;
		//		case EventType.GroupUnsubscribe:
		//			emailActivityEvent = jsonObject.ToObject<GroupUnsubscribeEvent>(serializer);
		//			break;
		//		case EventType.GroupResubscribe:
		//			emailActivityEvent = jsonObject.ToObject<GroupResubscribeEvent>(serializer);
		//			break;
		//		default:
		//			throw new Exception($"{eventTypeJsonProperty.ToString()} is an unknown event type");
		//	}

		//	return emailActivityEvent;
		//}
	}
}
