using System.Text.Json.Serialization;

namespace ZoomNet.Models.QualityOfService
{
	/// <summary>
	/// Quality of service data.
	/// </summary>
	public class PacketQualityOfServiceMetrics
	{
		/// <summary>
		/// Gets or sets the bitrate value.
		/// </summary>
		/// <value>
		/// Bitrate: The number of bits per second that can be transmitted along a digital network.
		/// </value>
		[JsonPropertyName("bitrate")]
		public string Bitrate { get; set; }

		/// <summary>
		/// Gets or sets the latency value.
		/// </summary>
		/// <value>
		/// Latency: The amount of time it takes for a pack to travel from one point to another. In Zoom's case, an audio, video, or screen share packet.
		/// </value>
		[JsonPropertyName("latency")]
		public string Latency { get; set; }

		/// <summary>
		/// Gets or sets the jitter value.
		/// </summary>
		/// <value>
		/// Jitter: The variation in the delay of received packets.
		/// </value>
		[JsonPropertyName("jitter")]
		public string Jitter { get; set; }

		/// <summary>
		/// Gets or sets the average loss value.
		/// </summary>
		/// <value>
		/// Average Loss: The average amount of packet loss, that is the percentage of packets that fail to arrive at their destination.
		/// </value>
		[JsonPropertyName("avg_loss")]
		public string AverageLoss { get; set; }

		/// <summary>
		/// Gets or sets the max loss value.
		/// </summary>
		/// <value>
		/// Max Loss: The max amount of packet loss, that is the max percentage of packets that fail to arrive at their destination.
		/// </value>
		[JsonPropertyName("max_loss")]
		public string MaxLoss { get; set; }
	}
}
