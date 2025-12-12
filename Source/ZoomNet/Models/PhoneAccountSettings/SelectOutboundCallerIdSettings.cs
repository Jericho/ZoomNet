using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow extensions to change outbound caller ID when placing calls.
	/// </summary>
	public class SelectOutboundCallerIdSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow extensions to hide outbound caller ID.
		/// </summary>
		[JsonPropertyName("allow_hide_outbound_caller_id")]
		public bool? AllowHideOutboundCallerId { get; set; }
	}
}
