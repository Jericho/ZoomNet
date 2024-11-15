using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models.CallHandlingSettings
{
	/// <summary>
	/// Call handling custom hours subsettings.
	/// </summary>
	public class CustomHoursSubsettings : CallHandlingSubsettingsBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the queue members are able to set their own business hours.
		/// This field allows queue members' business hours to override the default hours of the call queue.
		/// </summary>
		[JsonPropertyName("allow_members_to_reset")]
		public bool? AllowMembersToReset { get; set; }

		/// <summary>
		/// Gets or sets the type of custom hours.
		/// 1 — 24 hours, 7 days a week.
		/// 2 — Custom hours.
		/// </summary>
		[JsonPropertyName("type")]
		public byte? CustomHoursType { get; set; }

		/// <summary>
		/// Gets or sets the custom hours settings.
		/// </summary>
		[JsonPropertyName("custom_hours_settings")]
		public List<CustomHoursChildSubsettings> CustomHoursSettings { get; set; }

		/// <summary>
		/// Gets the type of sub-setting.
		/// </summary>
		[JsonIgnore]
		public override CallHandlingSubsettingsType SubsettingType => CallHandlingSubsettingsType.CustomHours;
	}
}
