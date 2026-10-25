using System.Text.Json.Serialization;

namespace ZoomNet.Models.QualityOfService
{
	/// <summary>CPU usage metrics.</summary>
	public record CpuUsage
	{
		/// <summary>Gets or sets the system maximum cpu usage.</summary>
		[JsonPropertyName("system_max_cpu_usage")]
		public string SystemMaxCpuUsage { get; init; }

		/// <summary>Gets or sets the Zoom average cpu usage.</summary>
		[JsonPropertyName("zoom_avg_cpu_usage")]
		public string AverageCpuUsage { get; init; }

		/// <summary>Gets or sets the Zoom maximum cpu usage.</summary>
		[JsonPropertyName("zoom_max_cpu_usage")]
		public string MaxCpuUsage { get; init; }

		/// <summary>Gets or sets the Zoom minimum cpu usage.</summary>
		[JsonPropertyName("zoom_min_cpu_usage")]
		public string MinCpuUsage { get; init; }
	}
}
