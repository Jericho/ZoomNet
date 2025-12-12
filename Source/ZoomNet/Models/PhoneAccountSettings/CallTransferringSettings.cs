using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow users to warm or blind transfer their calls.
	/// </summary>
	public class CallTransferringSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets call transfer restriction.
		/// </summary>
		[JsonPropertyName("call_transferring_type")]
		public CallRestrictionType? CallTransferringType { get; set; }
	}
}
