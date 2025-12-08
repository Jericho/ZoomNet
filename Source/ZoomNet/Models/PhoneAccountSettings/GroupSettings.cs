using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Group phone settings.
	/// </summary>
	public class GroupSettings : CommonAccountAndGroupSettings
	{
		/// <summary>
		/// Gets or sets settings that allow emergency calls from specific device types.
		/// </summary>
		[JsonPropertyName("allow_emergency_calls")]
		public AllowEmergencyCallsSettings AllowEmergencyCalls { get; set; }
	}
}
