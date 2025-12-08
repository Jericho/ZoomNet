using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow emergency calls from specific device types.
	/// </summary>
	public class AllowEmergencyCallsSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether users are allowed to make emergency calls from Zoom clients.
		/// </summary>
		[JsonPropertyName("allow_emergency_calls_from_clients")]
		public bool? AllowEmergencyCallsFromClients { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether users are allowed to make emergency calls from desk phones.
		/// </summary>
		[JsonPropertyName("allow_emergency_calls_from_deskphones")]
		public bool? AllowEmergencyCallsFromDeskphones { get; set; }
	}
}
