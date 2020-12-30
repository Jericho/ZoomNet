using Newtonsoft.Json;
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

			if (objectType == typeof(DayOfWeek) || objectType == typeof(DayOfWeek?))
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

			throw new Exception("Unable to deserialize the value");
		}
	}
}
