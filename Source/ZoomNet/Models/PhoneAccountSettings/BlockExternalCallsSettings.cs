using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to set rules for blocking external calls during business, closed, and holiday hours.
	/// </summary>
	public class BlockExternalCallsSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether external calls are blocked during business hours.
		/// </summary>
		[JsonPropertyName("block_business_hours")]
		public bool? BlockBusinessHours { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether external calls are blocked during closed hours.
		/// </summary>
		[JsonPropertyName("block_closed_hours")]
		public bool? BlockClosedHours { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether external calls are blocked during holiday hours.
		/// </summary>
		[JsonPropertyName("block_holiday_hours")]
		public bool? BlockHolidayHours { get; set; }

		/// <summary>
		/// Gets or sets the action when a call is blocked.
		/// </summary>
		[JsonPropertyName("block_call_action")]
		public BlockCallAction? BlockCallAction { get; set; }
	}
}
