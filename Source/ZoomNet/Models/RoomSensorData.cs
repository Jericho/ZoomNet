using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Room sensor data.</summary>
	public record RoomSensorData
	{
		/// <summary>Gets or sets the time when the sensor data is reported.</summary>
		[JsonPropertyName("date_time")]
		public DateTime Date { get; init; }

		/// <summary>Gets or sets the device Id.</summary>
		[JsonPropertyName("device_id")]
		public string DeviceId { get; init; }

		/// <summary>Gets or sets the type of sensor.</summary>
		[JsonPropertyName("sensor_type")]
		public RoomSensorType Type { get; init; }

		/// <summary>Gets or sets the value from the sensor.</summary>
		[JsonPropertyName("sensor_value")]
		public string Value { get; init; }
	}
}
