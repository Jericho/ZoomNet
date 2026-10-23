using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Simulive delay start settings.</summary>
	public class SimuliveDelayStart
	{
		/// <summary>Gets or sets a value indicating whether simulive needs to delay playback.</summary>
		[JsonPropertyName("enable")]
		public bool IsEnabled { get; set; }

		/// <summary>Gets or sets the delay time.</summary>
		/// <remarks>
		/// If the time unit is seconds, then the maximum value is 60 and the minimum value is 1.
		/// If the time unit is minutes, then the maximum value is 10 and the minimum value is 1.
		/// </remarks>
		[JsonPropertyName("time")]
		public int Time { get; set; }

		/// <summary>Gets or sets the delay time unit.</summary>
		[JsonPropertyName("timeunit")]
		public SimuliveDelayTimeUnit TimeUnit { get; set; }
	}
}
