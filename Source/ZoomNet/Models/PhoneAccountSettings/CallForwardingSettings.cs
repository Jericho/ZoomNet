using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow users to forward their calls to other numbers.
	/// </summary>
	public class CallForwardingSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets call forwarding restriction.
		/// </summary>
		[JsonPropertyName("call_forwarding_type")]
		public CallRestrictionType? CallForwardingType { get; set; }
	}
}
