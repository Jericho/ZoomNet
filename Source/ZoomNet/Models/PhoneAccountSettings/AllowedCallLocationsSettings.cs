using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow extension or user to make and accept calls and send SMS.
	/// </summary>
	public class AllowedCallLocationsSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow internal calls when outside of allowed locations.
		/// </summary>
		[JsonPropertyName("allow_internal_calls")]
		public bool? AllowInternalCalls { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether locations have been applied.
		/// </summary>
		[JsonPropertyName("locations_applied")]
		public bool? LocationsApplied { get; set; }
	}
}
