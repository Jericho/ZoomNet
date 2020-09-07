using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		[JsonProperty(PropertyName = "from")]
		public DateTime From { get; set; }

		/// <summary>
		/// Gets or sets the end date for this report.
		/// </summary>
		/// <value>The end date for this report.</value>
		[JsonProperty(PropertyName = "to")]
		public DateTime To { get; set; }
	}
}
