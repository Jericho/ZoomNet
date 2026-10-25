using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Usage of the CRC for the hour.</summary>
	public record CrcPortsHourUsage
	{
		/// <summary>Gets or sets the hour that the usage is for, in 24h format.</summary>
		[JsonPropertyName("hour")]
		public int Hour { get; init; }

		/// <summary>Gets or sets the maximum usage for the hour.</summary>
		[JsonPropertyName("max_usage")]

		public int MaxUsage { get; init; }

		/// <summary>Gets or sets the total usage for the hour.</summary>
		[JsonPropertyName("total_usage")]
		public int TotalUsage { get; init; }
	}
}
