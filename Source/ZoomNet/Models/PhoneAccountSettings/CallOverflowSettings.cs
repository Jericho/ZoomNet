using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow users to forward their calls to other numbers when a call is not answered.
	/// </summary>
	public class CallOverflowSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets call overflow restriction.
		/// </summary>
		[JsonPropertyName("call_overflow_type")]
		public CallRestrictionType? CallOverflowType { get; set; }
	}
}
