using System.Text.Json.Serialization;

namespace ZoomNet.Models.QualityOfService
{
	/// <summary>Quality of service data.</summary>
	public class PacketQualityOfServiceMetrics
	{
		/// <summary>Gets or sets the average loss value.</summary>
		[JsonPropertyName("avg_loss")]
		public string AverageLoss { get; set; }

		/// <summary>Gets or sets the bitrate value.</summary>
		[JsonPropertyName("bitrate")]
		public string Bitrate { get; set; }

		/// <summary>Gets or sets the jitter value.</summary>
		[JsonPropertyName("jitter")]
		public string Jitter { get; set; }

		/// <summary>Gets or sets the latency value.</summary>
		[JsonPropertyName("latency")]
		public string Latency { get; set; }

		/// <summary>Gets or sets the max loss value.</summary>
		[JsonPropertyName("max_loss")]
		public string MaxLoss { get; set; }
	}
}
