using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Setting that allows to enable call feedback and set min/max call duration.
	/// </summary>
	public class FeedbackDurationSetting
	{
		/// <summary>
		/// Gets or sets a value indicating whether setting is enabled.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets minimum call duration in seconds.
		/// </summary>
		[JsonPropertyName("min")]
		public int Min { get; set; }

		/// <summary>
		/// Gets or sets maximum call duration in seconds.
		/// </summary>
		[JsonPropertyName("max")]
		public int Max { get; set; }
	}
}
