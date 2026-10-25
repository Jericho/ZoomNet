using System.Text.Json.Serialization;

namespace ZoomNet.Models.QualityOfService
{
	/// <summary>Quality of service data.</summary>
	public record PacketQualityOfServiceMetrics
	{
		/// <summary>Gets or sets the average loss value.</summary>
		[JsonPropertyName("avg_loss")]
		public string AverageLoss { get; init; }

		/// <summary>Gets or sets the bitrate value.</summary>
		[JsonPropertyName("bitrate")]
		public string Bitrate { get; init; }

		/// <summary>Gets or sets the jitter value.</summary>
		[JsonPropertyName("jitter")]
		public string Jitter { get; init; }

		/// <summary>Gets or sets the latency value.</summary>
		[JsonPropertyName("latency")]
		public string Latency { get; init; }

		/// <summary>Gets or sets the max loss value.</summary>
		[JsonPropertyName("max_loss")]
		public string MaxLoss { get; init; }
	}
}
