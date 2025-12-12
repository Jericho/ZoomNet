using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow to define when the extension or user cannot make or accept calls and send SMS.
	/// </summary>
	public class RestrictedCallHoursSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow internal calls/SMS during restricted hours.
		/// </summary>
		[JsonPropertyName("allow_internal_calls")]
		public bool? AllowInternalCalls { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether restricted holiday hours has been applied.
		/// </summary>
		[JsonPropertyName("restricted_holiday_hours_applied")]
		public bool? RestrictedHolidayHoursApplied { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether restricted hours has been applied.
		/// </summary>
		[JsonPropertyName("restricted_hours_applied")]
		public bool? RestrictedHoursApplied { get; set; }

		/// <summary>
		/// Gets or sets timezone information.
		/// </summary>
		[JsonPropertyName("time_zone")]
		public TimezoneInfo Timezone { get; set; }
	}
}
