using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Setting that allows to enable call feedback and set min/max mean opinion score.
	/// </summary>
	public class FeedbackMeanOpinionScoreSetting
	{
		/// <summary>
		/// Gets or sets a value indicating whether setting is enabled.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets minimum MOS (mean opinion score).
		/// </summary>
		[JsonPropertyName("min")]
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public double Min { get; set; }

		/// <summary>
		/// Gets or sets maximum MOS (mean opinion score).
		/// </summary>
		[JsonPropertyName("max")]
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public double Max { get; set; }
	}
}
