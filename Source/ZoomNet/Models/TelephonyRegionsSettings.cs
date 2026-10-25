using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Telephony regions settings.</summary>
	public record TelephonyRegionsSettings
	{
		/// <summary>Gets or sets the Telephony regions provided by Zoom to select from.</summary>
		[JsonPropertyName("allowed_values")]
		public string[] AvailableRegions { get; init; }

		/// <summary>Gets or sets the telephony region where most participants call into or call from during a meeting.</summary>
		[JsonPropertyName("selection_value")]
		public string SelectedRegion { get; init; }
	}
}
