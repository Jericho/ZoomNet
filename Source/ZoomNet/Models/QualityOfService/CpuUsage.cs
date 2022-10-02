using System.Text.Json.Serialization;

namespace ZoomNet.Models.QualityOfService
{
	/// <summary>
	/// CPU usage metrics.
	/// </summary>
	public class CpuUsage
	{
		/// <summary>
		/// Gets or sets the Zoom minimum cpu usage.
		/// </summary>
		/// <value>
		/// The minimum value for Zoom CPU usage.
		/// </value>
		[JsonPropertyName("zoom_min_cpu_usage")]
		public string MinCpuUsage { get; set; }

		/// <summary>
		/// Gets or sets the Zoom average cpu usage.
		/// </summary>
		/// <value>
		/// The average value for Zoom CPU usage.
		/// </value>
		[JsonPropertyName("zoom_avg_cpu_usage")]
		public string AverageCpuUsage { get; set; }

		/// <summary>
		/// Gets or sets the Zoom maximum cpu usage.
		/// </summary>
		/// <value>
		/// The maximum value for Zoom CPU usage.
		/// </value>
		[JsonPropertyName("zoom_max_cpu_usage")]
		public string MaxCpuUsage { get; set; }

		/// <summary>
		/// Gets or sets the system maximum cpu usage.
		/// </summary>
		/// <value>
		/// The maximum value for system CPU usage.
		/// </value>
		[JsonPropertyName("system_max_cpu_usage")]
		public string SystemMaxCpuUsage { get; set; }
	}
}
