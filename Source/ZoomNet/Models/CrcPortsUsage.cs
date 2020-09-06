using Newtonsoft.Json;
using System;

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
		[JsonProperty(PropertyName = "date_time")]
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Gets or sets the hourly metrics for the port usage.
		/// </summary>
		/// <value>The hourly port usage.</value>
		[JsonProperty(PropertyName = "crc_ports_hour_usage")]
		public CrcPortsHourUsage[] HourlyUsage { get; set; }
	}
}
