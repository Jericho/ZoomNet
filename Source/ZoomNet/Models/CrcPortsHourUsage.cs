using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Usage of the CRC for the hour.
	/// </summary>
	public class CrcPortsHourUsage
	{
		/// <summary>
		/// Gets or sets the hour that the usage is for, in 24h format.
		/// </summary>
		/// <value>The hour in the day, during which the CRC was used. For example if the CRC was used at 11 pm, the value of this field will be 23.</value>
		[JsonPropertyName("hour")]
		public int Hour { get; set; }

		/// <summary>
		/// Gets or sets the maximum usage for the hour.
		/// </summary>
		/// <value>The maximum number of concurrent ports that are being used in that hour.</value>
		[JsonPropertyName("max_usage")]

		public int MaxUsage { get; set; }

		/// <summary>
		/// Gets or sets the total usage for the hour.
		/// </summary>
		/// <value>The total number of H.323/SIP connections in that hour.</value>
		[JsonPropertyName("total_usage")]
		public int TotalUsage { get; set; }
	}
}
