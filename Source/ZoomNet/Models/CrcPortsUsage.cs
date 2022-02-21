using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Hourly data of CRC usage.
	/// </summary>
	public class CrcPortsUsage
	{
		/// <summary>
		/// Gets or sets the date and time of the port usage.
		/// </summary>
		/// <value>The date and time of the port usage.</value>
		[JsonPropertyName("date_time")]
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Gets or sets the hourly metrics for the port usage.
		/// </summary>
		/// <value>The hourly port usage.</value>
		[JsonPropertyName("crc_ports_hour_usage")]
		public CrcPortsHourUsage[] HourlyUsage { get; set; }
	}
}
