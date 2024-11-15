using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Call distribution model for <see cref="CallHandlingSubsettings"/> settings.
	/// </summary>
	public class CallDistributionSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the maximum number of calls that can be handled simultaneously is less than half of the total amount of available call queue members.
		/// Note that the first incoming call may not be answered first.
		/// </summary>
		[JsonPropertyName("handle_multiple_calls")]
		public bool? HandleMultipleCalls { get; set; }

		/// <summary>
		/// Gets or sets the ringing duration for each member. Allowed values: 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60.
		/// </summary>
		[JsonPropertyName("ring_duration")]
		public int? RingDuration { get; set; }

		/// <summary>
		/// Gets or sets the call distribution ring mode.
		/// </summary>
		[JsonPropertyName("ring_mode")]
		public RingModeType? RingMode { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether:
		/// 1. Devices with Zoom app or client not launched and mobile phone with screen locked will be skipped;
		/// 2. Phone numbers added to user's call handling settings will be skipped.
		/// Required except for simultaneous ring mode.
		/// </summary>
		[JsonPropertyName("skip_offline_device_phone_number")]
		public bool? SkipOfflineDevicePhoneNumber { get; set; }
	}
}
