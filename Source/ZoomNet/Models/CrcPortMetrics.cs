using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Metrics for the CRC port usage.
	/// </summary>
	public class CrcPortMetrics
	{
		/// <summary>
		/// Gets or sets the start date for this report.
		/// </summary>
		/// <value>The start date for this report.</value>
		[JsonPropertyName("from")]
		public DateTime From { get; set; }

		/// <summary>
		/// Gets or sets the end date for this report.
		/// </summary>
		/// <value>The end date for this report.</value>
		[JsonPropertyName("to")]
		public DateTime To { get; set; }
	}
}
