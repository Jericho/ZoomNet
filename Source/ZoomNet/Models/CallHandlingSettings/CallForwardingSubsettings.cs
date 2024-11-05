using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Call forwarding subsettings.
	/// </summary>
	public class CallForwardingSubsettings : CallHandlingSubsettingsBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the user must press "1" before the call connects, when a call is forwarded to a personal phone number.
		/// Enable this option to ensure missed calls do not reach to your personal voicemail.
		/// Press 1 is always enabled and is required for callQueue type extension calls.
		/// </summary>
		[JsonPropertyName("sub_setting_type")]
		public bool? RequirePress1BeforeConnecting { get; set; }

		/// <summary>
		/// Gets or sets the list of call forwarding settings.
		/// </summary>
		[JsonPropertyName("call_forwarding_settings")]
		public List<CallForwardingChildSubsettings> CallForwardingSettings { get; set; }

		/// <summary>
		/// Gets the type of sub-setting.
		/// </summary>
		[JsonIgnore]
		public override CallHandlingSubsettingsType SubsettingType => CallHandlingSubsettingsType.CallForwarding;
	}
}
